window.addEventListener("DOMContentLoaded", (event) => {


	var maplatlng = document.getElementById("maplatlng");
	var splitinglat = maplatlng.value;
	var converter = [parseFloat(splitinglat.split(',')[0]), parseFloat(splitinglat.split(',')[1])];

	var mapfoall = L.map("mapid").setView(converter, 9);

	L.tileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
		maxZoom: 20,
		titleSize: 700,
	}).addTo(mapfoall);

	var coordarray = [
		converter, converter, converter
	];
	var namesarray = ["clinic one", "clinic two", "clinic three"];
	var imagesarray = ["../Try/media/one.jpg", "../Try/media/one.jpg"];

	var dep1 = document.getElementById("dep1");
	var dep2 = document.getElementById("dep2");
	var dep3 = document.getElementById("dep3");



	//var departsarray = [dep1, dep2, dep3];

	var len = coordarray.length;



	for (let i = 0; i < len; i++) {
		var marker = L.marker(coordarray[i]).addTo(mapfoall).bindPopup(popupname);

		var popupname = L.popup({
			closeonClick: true,
		}).setContent(" space " + namesarray[i] + "" + imagesarray[i]);

	}
});
