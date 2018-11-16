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
                    if(isset($_SESSION["normaluser_login"])){
                        echo '<li><a href="#">Welcome, <em>'. $login_user .'</em></a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="index.php">View Tenders</a></li>';
                        echo '<li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="announcement.php">Announcements</a></li>';
                        echo '<li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="searchTenders.php">Search Tenders</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="bookmark.php">Bookmark</a></li>';
                        echo '<li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="viewSurveyList.php">Survey</a></li>';
                        echo '<li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="logout.php">Logout</a></li>';
                    }else if(isset($_SESSION["user_login"])){
                        //if logged in as admin
                        echo '<li><a href="#">Welcome, <em>'. $login_user .'</em></a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="announcement.php">Announcement <span class="caret"></span></a>
                            <ul class="dropdown-menu">
                                <li><a href="announcement.php">View Announcements</a></li>
                                <li><a href="postAnnouncement.php">Post Announcement</a></li>
                            </ul>
                        </li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="#">Poll <span class="caret"></span></a>
                            <ul class="dropdown-menu">
                                <li><a href="poll.php">View Polls</a></li>
                                <li><a href="createPoll.php">Create Poll</a></li>
                            </ul>
                        </li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        
                        echo '<li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="viewSurvey.php">Survey <span class="caret"></span></a>
                            <ul class="dropdown-menu">
                                <li><a href="viewSurvey.php">View Surveys</a></li>
                                <li><a href="postSurvey.php">Create Survey</a></li>
                            </ul>
                        </li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="index.php">View Tenders</a></li>';
                        echo '<li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        if (isset($_SESSION["user_role"]) && $_SESSION["user_role"] == "admin") {
                            echo '<li><a href="manage_users.php">Manage Users</a></li>
                            <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        }
                        echo '<li><a href="logout.php">Logout</a></li>';
                    }else{
                        echo '<li><a href="index.php">View Tenders</a></li>';
                        echo '<li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="searchTenders.php">Search Tenders</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="login.php">Login</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="registration.php">Register</a></li>
                        <li><a href="#" class="btn disabled hidden-xs">|</a></li>';
                        echo '<li><a href="adminLogin.php">Administrator Login</a></li>';
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