<?php

 $db = new mysqli('localhost', 'root','', 'control_server');
 if(mysqli_connect_errno()) {
      
    exit;
 }
	
$name = $_POST["name"];
$ip = $_POST["ip"];
$query = "INSERT INTO clients(name, ip) VALUES(?,?)";
$stmt = $db->prepare($query);
$stmt->bind_param('ss',$name,$ip);
$stmt->execute();
$db->close();

?>