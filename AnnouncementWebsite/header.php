<?php 
    include("session.php");

    //Display the navigation bar
    echo 
        '<nav class="navbar navbar-default navbar-static-top" role="navigation">
            <div class="container-fluid">
                <div class="navbar-header">
                    <!--3 bar icon-->
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#navbar-to-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>

                    <a class="navbar-brand" href="index.php"><em>SESCo Tender Notices Announcements</em></a>
                </div>

                <div class="collapse navbar-collapse" id="navbar-to-collapse">
                    <ul class="nav navbar-nav navbar-right">';
                    if (isset($_SESSION["user_login"])) {
                        echo '<li><a href="manage_profile.php">Welcome, <em>'. $_SESSION["user_login"] .'</em></a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                    }
                        echo '<li><a href="index.php">Announcements</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                
                //display if adminnistrator logged in
                if (!isset($_SESSION["user_login"])){

                    echo '<li><a href="login.php">Administrator Login</a></li>';
                } else {
                    echo '<li><a href="postAnnouncement.php">Posts Announcement</a></li>
                    <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                    if (isset($_SESSION["user_role"]) && $_SESSION["user_role"] == "admin") {
                        echo '<li><a href="manage_users.php">Manage Users</a></li>
                            <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                    }
                    
                    echo '<li><a href="logout.php">Logout</a></li>';
                    
                }      
                    echo '</ul>
                </div>
            </div>
        </nav>
        
        <div>
            <img class="sescobanner" src="images/banner.jpg"/>
        </div>]';