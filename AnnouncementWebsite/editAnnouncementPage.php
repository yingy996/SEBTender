<!DOCTYPE html>
<html data-ng-app="">
<head>
    <title>Deallo Craft House</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initialscale=1.0"/>
    <!-- Bootstrap -->
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/stylesheet.css" rel="stylesheet" />
    
</head>
<body id="loginpg"> <!--full page background img -->
    <?php 
		include("header.php");
        include("process_editAnnouncementPage.php")
    ?>
    
    <div class="container-fluid">
        <div class="row">
            <div class="col-xs-4 col-xs-offset-4">
                <form id="postAnnouncement" name="editAnnouncement" method="post" action="" novalidate role="form">
                    <fieldset>
                        <legend>Edit an announcement</legend>
                        <?php if(!empty($success_message)) { ?>	
                    <div class="alert alert-success">
                    <?php if(isset($success_message)) echo $success_message; ?></div>
                    <?php } ?>
                    <?php if(!empty($error_message)) { ?>	
                    <div class="alert alert-danger"><?php if(isset($error_message)) echo $error_message; ?></div>
                    <?php } ?>
                        <div class="form-group">
                            <label for="title">Announcement Title:</label>
                            <input type="text" class="form-control" id="title" name="title" placeholder="Edit Announcement Title" value="<? echo $oriannouncementTitle ?>"required/><span class="error"><?php if($announcement_title != "") echo "<p class='alert alert-danger'>" . $announcement_titleerr . "</p>";?></span>
                        </div>

                        <div class="form-group">
                            <label for="content">Announcement Content:</label>
                            <textarea class="form-control" id="content" name="content" placeholder="Edit Announcement Content" value="<? echo $oriannouncementContent ?>" required></textarea><span class="error"><?php if($announcement_content != "") echo "<p class='alert alert-danger'>" . $announcement_contenterr . "</p>";?></span>
                        </div>
                        <p><input type="submit" name="editAnnouncementbutton" class="btn btn-default" id="editAnnouncementbutton" value="Edit Announcement"/></p>

                        
                    </fieldset>
                    
                </form>
            </div>
        </div>
    </div>
    
    <!-- Footer -->
    <?php 
        include("footer.php");
    ?>
    
    <!-- jQuery – required for Bootstrap's JavaScript plugins) -->
    <script src="js/jquery.min.js"></script>
    <!-- All Bootstrap plug-ins file -->
    <script src="js/bootstrap.min.js"></script>
    <!--Basic AngularJS-->
    
</body>
</html>