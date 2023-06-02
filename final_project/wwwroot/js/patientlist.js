window.addEventListener("DOMContentLoaded", (event) => {
	document.getElementById("sub_list_header").addEventListener("click", function () {
		if (document.getElementById("sub_list").classList.contains("sub_list_show")) {
			document.getElementById("sub_list").classList.remove("sub_list_show");
		}
		else {
			document.getElementById("sub_list").classList.add("sub_list_show");
		}
	});
});