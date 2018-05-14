<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>SESCO eTender Announcements</title>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initialscale=1.0"/>
        <!-- Bootstrap -->
        <link href="css/bootstrap.min.css" rel="stylesheet" />
        <link href="css/stylesheet.css" rel="stylesheet" />

    </head>
    <body id="loginpg">
        <?php 
        include("header.php");
        
        include("process_changePassword.php");
        ?>
        
        <!-- Body -->
        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-4 col-xs-offset-4">
                    <form id="addAdmin" name="addAdmin" method="post" action="" novalidate role="form">
                        <fieldset>
                            <legend>Change Password</legend>
                            <?php if(!empty($success_message)) { ?>	                           
                                <div class="alert alert-success"><?php if(isset($success_message)) echo $success_message; ?></div>                   
                            <?php } ?>
                            
                            <?php if(!empty($error_message)) { ?>	                            
                                <div class="alert alert-danger"><?php if(isset($error_message)) echo $error_message; ?></div>
                            <?php } ?>

                            <div class="form-group">
                                <label for="oldPassword">Old Password:</label>
                                <input type="password" class="form-control" id="oldPassword" name="oldPassword" required/>
                                <span class="error"><?php if($oldPasswordError != "") echo "<p class='alert alert-danger'>" . $oldPasswordError . "</p>";?></span>
                            </div>
                            
                            <div class="form-group">
                                <label for="newPassword">New Password:</label>
                                <input type="password" class="form-control" id="newPassword" name="newPassword" required/>
                                <span class="error"><?php if($newPasswordError != "") echo "<p class='alert alert-danger'>" . $newPasswordError . "</p>";?></span>
                            </div>
                            
                            <div class="form-group">
                                <label for="password">Confirm New Password:</label>
                                <input type="password" class="form-control" id="confPassword" name="confPassword" required/>
                                <span class="error"><?php if($confPasswordError != "") echo "<p class='alert alert-danger'>" . $confPasswordError . "</p>";?></span>
                            </div>
                            <p><input type="submit" name="updateBtn" class="btn btn-default" id="updateBtn" value="Update"/></p>
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