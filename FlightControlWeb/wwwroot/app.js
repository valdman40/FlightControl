// A $( document ).ready() block.
$(document).ready(function () {
    let isPressedId;
    let blackIcon = L.icon({
        iconUrl: 'plane.png',
        iconSize: [20, 25],
        // iconAnchor: [1, 24],
        popupAnchor: [-3, -76],
    });

    let activeIcon = L.icon({
        iconUrl: 'redIcon.png',
        iconSize: [20, 25],
        // iconAnchor: [1, 24],
        popupAnchor: [-3, -76],
    });
    let map = L.map('map', { minZoom: 3, }).setView([33, 31], 2);
    mapLink =
        '<a href="http://openstreetmap.org">OpenStreetMap</a>';
    L.tileLayer(
        'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; ' + mapLink + ' Contributors',
        maxZoom: 18,
    }).addTo(map);

    L.tileLayer('https://api.maptiler.com/maps/streets/{z}/{x}/{y}.png?key=dwAzStO5BTv3oNPggfCv', {
        attribution: '<a href="https://www.maptiler.com/copyright/" target="_blank">&copy; MapTiler</a> <a href="https://www.openstreetmap.org/copyright" target="_blank">&copy; OpenStreetMap contributors</a>'
    }).addTo(map);

    setTimeout(function () { map.invalidateSize() }, 10);
    map.on('click', function () {
        removeEmphasis();
    });
    function calculateEndTime(dateAndTime, FlightPlan) {
        var date = new Date(dateAndTime);
        let timespan_seconds = 0;
        for (let i = 0; i < FlightPlan.segments.length; i++) {
            timespan_seconds += FlightPlan.segments[i].timespan_seconds;
        }
        console.log(timespan_seconds);
        date.setSeconds(date.getSeconds() + timespan_seconds);


        var set = date.getSeconds();
        console.log(set);


        var resEndTime = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        console.log(resEndTime);
        return resEndTime;

    }
    function writeFlightsDetails(FlightPlan, Flight, i, flagIsExternal) {

        let passengers = FlightPlan.passengers;
        let segments_len = FlightPlan.segments.length;
        let time = new Date(FlightPlan.initial_location.date_time);
        let start_time = time.getHours() + ':' + time.getMinutes() + ':' + time.getSeconds();
        console.log("start" + start_time);
        let end_time = calculateEndTime(FlightPlan.initial_location.date_time, FlightPlan);
        let initia_log = FlightPlan.initial_location.longitude;
        let initial_lat = FlightPlan.initial_location.latitude;
        let initial_location = "longitude: " + initia_log + "   latitude: " + initial_lat;
        let final_log = FlightPlan.segments[segments_len - 1].longitude;
        let final_lat = FlightPlan.segments[segments_len - 1].latitude;
        let final_location = "longitude: " + final_log + "   latitude: " + final_lat;
        let company_name = Flight[i].company_name;
        if (flagIsExternal) {
            document.getElementById("origin").innerHTML = "external flight";
        } else {
            document.getElementById("origin").innerHTML = "internal flight";
        }
        document.getElementById("company").textContent = company_name;
        document.getElementById("passengers").textContent = passengers;
        document.getElementById("starttime").textContent = start_time;
        document.getElementById("endtime").textContent = end_time;
        document.getElementById("startloc").textContent = initial_location;
        document.getElementById("endloc").textContent = final_location;
    }
    function ListCreator(data) {
        let ul = document.getElementById("flightsButtons");
        for (let i = 0; i < data.length; i++) {
            let li = document.createElement("li");
            li.appendChild(document.createTextNode("Flight" + data[i].flight_id + " " + data[i].company_name));
            //List that can be closed using x
            let span = document.createElement("SPAN");
            let txt = document.createTextNode("x");
            span.className = "close";
            span.appendChild(txt);
            li.isPressed = false;
            li.setAttribute("id", data[i].flight_id);
            ul.appendChild(li);
            // if its not external - add option to remove using x
            if (data[i].is_external === false) {
                li.appendChild(span);
            }
            //if this flight is pressed - so mark red
            if (isPressedId === data[i].flight_id) {
                redFount(data[i].flight_id);
            }
            li.addEventListener("click", () => {
                ListClicked(data, i);
            });
        }
    }

    function ListClicked(data, i) {
        // remove marks
        removeEmphasis();
        //update isPressedId let
        isPressedId = data[i].flight_id;
        let flights_plan = getFlightsPlan(data[i].flight_id);
        // data is Fligt js
        if (data[i].is_external === false) {
            //document.getElementById("infoFlightdetails").innerHTML = "Flight id: " + data[i].flight_id + " company_name :" + data[i].company_name;
            writeFlightsDetails(flights_plan, data, i, 0);
        } else { // its external flight
            //document.getElementById("infoFlightdetails").innerHTML = "External: " + " Flight id:" + data[i].flight_id + " company_name :" + data[i].company_name;
            writeFlightsDetails(flights_plan, data, i, 1);
        }

        showPath(data[i]["flight_id"], flights_plan);
        markerFlightsDict[data[i]["flight_id"]].setIcon(activeIcon);
        // mark red
        redFount(data[i].flight_id);
    }



    let date = new Date('2019-05-20T21:27:07Z');
    function giveMeByTime() {
        $.ajax({
            url: `../api/Flights?relative_to=${date}`,
            type: 'GET',
            success: function (result) {
                console.log(result);
                date.setSeconds(date.getSeconds() + 1);
                setTimeout(function () { giveMeByTime(); }, 1000);
            }
        });
    }
    function getFlightsPlan(flightID) {
        let flightPlan;
        $.ajax({
            url: `../api/FlightPlan/${flightID}`,
            type: 'GET',
            success: function (result) {
                flightPlan = result;
            },
            async: false
        });
        console.log(flightPlan);
        return flightPlan;
    }
    let shelterMarkers = L.featureGroup();
    function showPath(flightID, flights_plan) {
        shelterMarkers.clearLayers();
        map.addLayer(shelterMarkers);
        //let flightPlan = getFlightsPlan(flightID);
        let coords = [];
        coords.push([flights_plan.initial_location.latitude, flights_plan.initial_location.longitude]);
        for (let i = 0; i < flights_plan.segments.length; i++) {
            coords.push([flights_plan.segments[i].latitude, flights_plan.segments[i].longitude]);
            let polyline = L.polyline(coords, { color: 'red' }).addTo(shelterMarkers);
            // zoom the map to the polyline
            ///map.fitBounds(polyline.getBounds());
        }
    }
    // initialize a dictionary between flight and the icon corresponding to the map
    let markerFlightsDict = {}
    let group = L.layerGroup();/////
    function DrawIcons(data) {
        group.clearLayers();////
        for (let i = 0; i < data.length; i++) {
            let lon = data[i]["longitude"];
            let lat = data[i]["latitude"];
            let marker = L.marker([lat, lon], { icon: blackIcon });
            if (data[i].flight_id === isPressedId) {
                marker.setIcon(activeIcon);
            }
            marker.addTo(map);
            marker._leaflet_id = data[i].flight_id;
            map.addLayer(marker);
            marker.addTo(group);
            map.addLayer(group); //for removing single plane
            //click on airplane marker

            marker.addEventListener("click", () => {
                IconClicked(data, i, marker);
            });
            markerFlightsDict[data[i]["flight_id"]] = marker;
        }
        ListCreator(data);
    }

    function IconClicked(data, i, marker) {
        removeEmphasis();
        //update isPressedId let
        isPressedId = data[i].flight_id;
        // set marker icon
        marker.setIcon(activeIcon);
        let flights_plan = getFlightsPlan(data[i].flight_id);
        showPath(data[i]["flight_id"], flights_plan);
        writeFlightsDetails(flights_plan, data, i);
        // mark red
        redFount(data[i].flight_id);
    }



    function redFount(datai) {
        // console.log(datai);
        let btn = document.createElement("BUTTON");
        let attr = document.createAttribute("class");
        attr.value = "markButtonClass";
        let h = document.getElementById(datai);
        h.setAttributeNode(attr);
        h.isPressed = true;
    }
    function removeEmphasis() {
        let liButton = document.getElementsByTagName("li");
        ClearTable();
        ///////////////document.getElementById("infoFlightdetails").innerHTML = "";
        isPressedId = '';
        for (let i = 0; i < liButton.length; i++) {
            liButton[i].isPressed = false;
            let attr1 = document.createAttribute("class");
            attr1.value = "unpressed";
            let elementInFlightsList = document.getElementById(liButton[i].id);
            elementInFlightsList.setAttributeNode(attr1); // will unmark the element
            let flightIcon = liButton[i].id;
            markerFlightsDict[String(flightIcon)].setIcon(blackIcon); // turning to the black icon
            map.removeLayer(shelterMarkers); // remove segments
        }
    }

    function ClearTable() {
        document.getElementById("company").textContent = "";
        document.getElementById("passengers").textContent = "";
        document.getElementById("starttime").textContent = "";
        document.getElementById("endtime").textContent = "";
        document.getElementById("startloc").textContent = "";
        document.getElementById("endloc").textContent = "";
        document.getElementById("origin").innerHTML = "";
    }



    function CloseButtonClicked() {
        let close = document.getElementsByClassName("close");
        for (let i = 0; i < close.length; ++i) {
            //click on x button
            close[i].onclick = function () {
                let div = this.parentElement;
                if (div.isPressed === false) { // so we want to delete a flight
                    map.removeLayer(markerFlightsDict[String(div.id)]);
                    div.style.display = "none";
                    deleteElement(div.id);
                }
                else {  // so we want to unmark a flight
                    removeEmphasis();
                }
                event.cancelBubble = true;
            }
        }
    }
    function deleteElement(inputID) {
        console.log(inputID);
        $.ajax({
            url: `../api/Flights/${inputID}`,
            type: 'DELETE',
            success: function (result) {
                alert('success');
            },
            fail: function (xhr, textStatus, errorThrown) {
                alert('failed' + errorThrown);
            }
        });
    }
    function clickDispaly() {
        let div = this.parentElement;
        if (div.isPressed == false) {
            div.style.display = "none";
        }
        else {
            div.isPressed = false;
            div.classList.toggle('checked');
        }
    }
    let closebtns = document.querySelectorAll(".close");
    Array.from(closebtns).forEach(item => {
        item.addEventListener("click", () => {
            item.parentElement.style.display = "none";
        });
    });
    //2018-06-25T17:26:45Z
    let dt = new Date("25 June 2018 17:26:45 UTC");
    function recoursiveAjaxRequest() {
        let date = dt.toISOString().split('.')[0] + "Z";
        $.ajax({
            url: `../api/Flights?relative_to=${date}&sync_all`,
            type: 'GET',
            success: function (result) {
                Flights = result;
                dt.setSeconds(dt.getSeconds() + 4);
                clearFlights();
                DrawIcons(Flights);
                CloseButtonClicked();
                setTimeout(function () {
                    recoursiveAjaxRequest()
                }, 4000)
            },
        });
    }
    recoursiveAjaxRequest();
    function clearFlights() {
        $('ul li').remove();
        markerFlightsDict = {};
        let x = document.getElementsByTagName("li");
        //document.getElementById("infoFlightdetails").innerHTML = "";
        for (let i = 0; i < x.length; i++) {
            map.removeLayer(markerFlightsDict[String(x[i].id)]);
            //x[i].style.display = "none";
        }
    }
});