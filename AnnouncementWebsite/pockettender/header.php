<?php 
    include("session.php");

    //Display the navigation bar
    echo 
        '<nav class="navbar navbar-inverse navbar-static-top" role="navigation">
        <div class="container-fluid">
            <div class="navbar-header">
                <!--3 bar icon-->
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#navbar-to-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                
                <a class="navbar-brand" href="index.php">Pocket Tender</a>
            </div>
            
            <div class="collapse navbar-collapse" id="navbar-to-collapse">
                    <ul class="nav navbar-nav navbar-right">';
                    if(isset($login_user)){
                        echo '<li><a href="#">Welcome, <em>'. $login_user .'</em></a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="index.php">View Tenders</a></li>';
                        echo '<li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="searchTenders.php">Search Tenders</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="bookmark.php">Bookmark</a></li>';
                        echo '<li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="logout.php">Logout</a></li>';
                    }else {
                        echo '<li><a href="index.php">View Tenders</a></li>';
                        echo '<li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="searchTenders.php">Search Tenders</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="login.php">Login</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="registration.php">Register</a></li>';
                    }            
        echo '      </ul>
                </div>
            </div>
        </nav>';
               
function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>