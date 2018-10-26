<?php 
    include("session.php");

    //Display the navigation bar
    echo 
        '<nav class="navbar navbar-inverse">
        <div class="container-fluid">
            <div class="navbar-header">
              <a class="navbar-brand" href="index.php">Pocket Tender</a>
            </div>
            <div class="collapse navbar-collapse" id="navbar-to-collapse">
                    <ul class="nav navbar-nav navbar-right">';
                    if(isset($_SESSION["normaluser_login"])){
                        echo '<li><a href="#"class="btn disabled hidden-xs">Welcome, <em>'. $_SESSION["normaluser_login"] .'</em></a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        
                       echo '<li><a href="logout.php">Logout</a></li>';
                    }else {
                       echo '<li><a href="login.php">Login</a></li>
                       <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                       echo '<li><a href="registration.php">Register</a></li>';
                    }
            


echo'
        </div>
    </nav>';
                
                     
               
function sanitizeInput($data) {
        $data = trim($data);
        $data = stripslashes($data);
        $data = htmlspecialchars($data);
        
        return $data;
    }
