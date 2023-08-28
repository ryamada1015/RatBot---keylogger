<?php

if(file_exists("cmdstr.txt")){
	$f = fopen("cmdstr.txt", "r");
	echo fread($f, filesize("cmdstr.txt"));
	// delete file
	unlink("cmdstr.txt"); 
} else echo "nofile";

?>