//obtener la dirección en donde nos encontramos por medio de geolocalización
function getGeo() {
    //Si el browser está habilitado para geolocalización
    if (navigator && navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(geoOK, geoKO);
    }
}

//Se ejecuta cuando getCurrentPosition tiene éxito
function geoOK(position) {
    showLatLong(position.coords.latitude, position.coords.longitude);
}

//Se ejecuta en caso de un error en getCurrentPosition
function geoKO(err) {
    if (err.code == 1) { error('El usuario denegó el permiso'); }
    else if (err.code == 2) { error('La ubicación no se puede determinar'); }
    else if (err.code == 3) { error('TimeOut'); }
    else { error('Error: ' + err.code); }
}

function showLatLong(lat, longi) {
    $("#direccion").html("latitud: " + lat + " longitud: " + longi);
    //Después calculamos la dirección física
    var geocoder = new google.maps.Geocoder();
    var milocalizacion = new google.maps.LatLng(lat, longi);
    geocoder.geocode({ 'latLng': milocalizacion }, processGeocoder);
}

function processGeocoder(result, status) {
    if (status == google.maps.GeocoderStatus.OK) {
        //si traigo información
        if (result[0]) {
            //despliego la dirección
            var direccion = result[0].formatted_address;
            $("#direccion").html(direccion);
        }
        else {
            error("Google no retornó resultados");
        }
    }
    else {
        error("Google no retornó resultados");
    }
}

function dibujaMapa(lat, longi) {
    var coordenadas = { Lat: 0, Lng: 0 };

    function localizacion(posicion) {
        if (lat == 0) {
            //Saco mis coordenadas de geolocalización
            coordenadas = {
                Lat: posicion.coords.latitude,
                Lng: posicion.coords.longitude
            };
        }
        else {
            //Asigno a mis coordenadas los parámetros recibidos
            coordenadas = {
                Lat: lat,
                Lng: longi
            };
        }

        //Definir las opciones para dibujar el mapa
        var myLatLng = new google.maps.LatLng(coordenadas.Lat, coordenadas.Lng);
        var mapOptions = {
            zoom: 16,
            center: myLatLng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        //Cargar el mapa en el div indicado
        var map = new google.maps.Map(document.getElementById('mapa'), mapOptions);
        //Definir el marcardor (de google) en la posición indicada
        var marker = new google.maps.Marker({
            position: myLatLng,
            title: "Coshipigua",
            icon:
                {
                    url: "/img/Coshipigua.png",
                    scaledSize: new google.maps.Size(70, 70),
                    animation: google.maps.Animation.BOUNCE
                }
        });

        marker.setMap(map);
        $("#mapa").css("height", 350);
        var panorama = new google.maps.StreetViewPanorama(document.getElementById("street"), { position: myLatLng, pov: { heading: 90, pitch: 5 } });
        map.setStreetView(panorama);
        $("#street").css("height", 300);

    }

    if (navigator && navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(localizacion, error);
    }
}

function error(error) {
    alert(error);
}