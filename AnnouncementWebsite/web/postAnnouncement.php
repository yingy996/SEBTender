<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>SESCo eTender Mobile Application Announcements</title>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initialscale=1.0"/>
        <!-- Bootstrap -->
        <link href="../css/bootstrap.min.css" rel="stylesheet" />
        <link href="../css/stylesheet.css" rel="stylesheet" />

    </head>
    <body id="loginpg"> <!--full page background img -->
        <?php 
        @ob_start();
        include("header.php");
        include("process_postAnnouncement.php")
        ?>

        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-4 col-xs-offset-4">
                    <form id="postAnnouncement" name="postAnnouncement" method="post" action="" novalidate role="form">
                        <fieldset>
                            <legend>Posts an announcement</legend>
                            <?php if(!empty($success_message)) { ?>	
                            <div class="alert alert-success">
                                <?php if(isset($success_message)) echo $success_message; ?></div>
                            <?php } ?>
                            <?php if(!empty($error_message)) { ?>	
                            <div class="alert alert-danger"><?php if(isset($error_message)) echo $error_message; ?></div>
                            <?php } ?>
                            <div class="form-group">
                                <label for="title">Announcement Title:</label>
                                <input type="text" class="form-control" id="title" name="title" placeholder="Enter Announcement Title" required/><span class="error"><?php if($announcement_titleerr != "") echo "<p class='alert alert-danger'>" . $announcement_titleerr . "</p>";?></span>
                            </div>

                            <div class="form-group">
                                <label for="content">Announcement Content:</label>
                                <textarea class="form-control" id="content" name="content" placeholder="Enter Announcement Content" required></textarea><span class="error"><?php if($announcement_contenterr != "") echo "<p class='alert alert-danger'>" . $announcement_contenterr . "</p>";?></span>
                            </div>
                            <p><input type="submit" name="postAnnouncementbutton" class="btn btn-default" id="postAnnouncementbutton" value="Posts Announcement"/></p>


                        </fieldset>

                    </form>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 text-center">
                    <hr/>
                    <p>&copy; Developed by Team <em>Dinosaur</em> | Swinburne University of Technology Sarawak</p>
                </div>
            </div>
        </div>


        <!-- jQuery – required for Bootstrap's JavaScript plugins) -->
        <script src="../js/jquery.min.js"></script>
        <!-- All Bootstrap plug-ins file -->
        <script src="../js/bootstrap.min.js"></script>
        <!--Basic AngularJS-->

    </body>
</html>