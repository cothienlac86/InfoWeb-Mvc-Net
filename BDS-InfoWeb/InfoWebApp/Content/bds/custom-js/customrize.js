$(function () {
	//$('#datetimepicker1').datetimepicker();
	checkResize();
	$(window).resize(function() {
	  checkResize();
	});
});

function checkResize(){
	if($('.icon-bar').is(':visible')){ 
		$("#btnSearch").css("margin-top","10px");  
	  }else{
		  $("#btnSearch").css("margin-top","0");
	  }
}