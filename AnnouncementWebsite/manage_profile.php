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
    <body>
        <?php 
            include("header.php");
            include("process_manageProfile.php");
        
            if (!isset($_SESSION["user_login"])) {
                header("location: login.php");
                exit();
            }
        ?>
        
        <div class="container">
            <div class="row">
                <div class="col-xs-12">
                    <h1>Manage Profile</h1>
                    <hr/>
                </div>
            </div>
            
            <div class="row">
                <div class="col-xs-12">                 
                    
                    <form action="editUser.php" method="post">
                        <p>
                            Edit My Profile: 
                            <input type="submit" id="editButton" name="editButton" class="btn btn-default" value="Edit Profile"/>
                        </p>
                        
                        <input type="hidden" id="username" name="username" value="<?php echo $login_user; ?>"/>
                    </form>
                    <br/>
                </div>
            </div>
            
            <div class="row">
                <div class="col-xs-12">                 
                    <p>Change Password: <a href="change_password.php" class="btn btn-default">Change Password</a></p>
                    <br/>
                </div>
            </div>
            
            <hr/>
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