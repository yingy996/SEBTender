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
                        
                        echo '<li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="index.php">Announcement <span class="caret"></span></a>
                            <ul class="dropdown-menu">
                                <li><a href="index.php">View Announcements</a></li>
                                <li><a href="postAnnouncement.php">Post Announcement</a></li>
                            </ul>
                        </li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        
                        echo '<li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="index.php">Poll <span class="caret"></span></a>
                            <ul class="dropdown-menu">
                                <li><a href="poll.php">View Polls</a></li>
                                <li><a href="createPoll.php">Create Poll</a></li>
                            </ul>
                        </li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        
                        echo '<li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="survey.php">Survey <span class="caret"></span></a>
                            <ul class="dropdown-menu">
                                <li><a href="survey.php">View Surveys</a></li>
                                <li><a href="postSurvey.php">Create Survey</a></li>
                            </ul>
                        </li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                    } else {
                        echo '<li><a href="index.php">Announcements</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        
                        echo '<li><a href="poll.php">Poll</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        
                        echo '<li><a href="postSurvey.php">Survey</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                    }
                             
                //display "Login" link if administrator not logged in
                if (!isset($_SESSION["user_login"])){

                    echo '<li><a href="login.php">Administrator Login</a></li>';
                } else {
                    
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
        </div>';