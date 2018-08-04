
// displays a wait spinner
function ShowWaitSpinner() {

	$.blockUI({
		css: {
			border: "none",
			backgroundColor: "transparent",
			opacity: .9,
			color: "#000"
		},
		overlayCSS: { backgroundColor: "#686868" },
		message: $("#spinner")
	});

	setTimeout($.unblockUI, 500000);
};

$(document).ready(function() {

	// handles the temp spinner button click event to display the spinner
	// sets a 120 second timeout
	$("#goButton").on("click", function() {
		ShowWaitSpinner();
	});

});
