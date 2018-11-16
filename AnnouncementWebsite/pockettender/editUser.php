<?php 
include("header.php");
include("process_editUser.php");
//include("process_addAdmin.php");
?>
<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>SESCO eTender Announcements</title>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initialscale=1.0"/>
        <!-- Bootstrap -->
        <link href="../css/bootstrap.min.css" rel="stylesheet" />
        <link href="../css/stylesheet.css" rel="stylesheet" />

    </head>
    <body id="loginpg">
        
        <!-- Body -->
        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-4 col-xs-offset-4">
                    <form id="addAdmin" name="addAdmin" method="post" action="" novalidate role="form">
                        <fieldset>
                            <legend>Update User Information</legend>
                            <?php if(!empty($success_message)) { ?>	                           
                                <div class="alert alert-success"><?php if(isset($success_message)) echo $success_message; ?></div>                   
                            <?php } ?>
                            
                            <?php if(!empty($error_message)) { ?>	                            
                                <div class="alert alert-danger"><?php if(isset($error_message)) echo $error_message; ?></div>
                            <?php } ?>
                            
                            <div class="form-group">
                                <label for="name">Name:</label>
                                <input type="text" class="form-control" id="name" name="name" placeholder="Enter name" value="<?php if($name != "") echo $name; ?>" required/>
                                <span class="error"><?php if($nameError != "") echo "<p class='alert alert-danger'>" . $nameError . "</p>";?></span>
                            </div>
                            
                            <div class="form-group">
                                <label for="email">Email:</label>
                                <input type="email" class="form-control" id="email" name="email" placeholder="Enter email" value="<?php if($email != "") echo $email; ?>" required/>
                                <span class="error"><?php if($emailError != "") echo "<p class='alert alert-danger'>" . $emailError . "</p>";?></span>
                            </div>
                            
                            <div class="form-group">
                                <label for="role">Role:</label>
                                <select class="form-control" name="role" required <?php if($login_user == $username) { echo "disabled"; } ?>>
                                    <option value="admin" <?php if($role == "admin") { echo "selected"; } ?>>Administrator</option>
                                    <option value="editor" <?php if($role == "editor") { echo "selected"; } ?>>Editor</option>
                                </select>
                                <?php if($login_user == $username) {echo '<input type="hidden" name="role" value="' . $role . '"/>';} ?>
                            </div>
                            
                            
                            <div class="form-group">
                                <label for="displayUsername">Username:</label>
                                <input type="text" disabled class="form-control" id="displayUsername" name="displayUsername" placeholder="Enter username" value="<?php if($username != "") echo $username; ?>"/>
                                <?php echo '<input type="hidden" name="username" value="' . $username . '"/>' ?>
                                <span class="error"><?php if($usernameError != "") echo "<p class='alert alert-danger'>" . $usernameError . "</p>";?></span>
                            </div>

                            
                            
                            <p><input type="submit" name="editAdminBtn" class="btn btn-default" id="editAdminBtn" value="Update"/></p>
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