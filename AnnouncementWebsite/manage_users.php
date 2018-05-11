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
    <body> <!--full page background img -->
        <?php 
            include("header.php");
            include("process_manageUsers.php");
        
            if (isset($_SESSION["user_login"])) {
                if(isset($_SESSION["user_role"])) {
                    if ($_SESSION["user_role"] != "admin") {
                        header("location: index.php");
                        exit();
                    }
                }
            } else {
                header("location: login.php");
                exit();
            }
        ?>
        
        <div class="container">
            <div class="row">
                <div class="col-xs-12">
                    <h1>Manage Admin Users</h1>
                    <hr/>
                </div>
            </div>
            
            <div class="row">
                <div class="col-xs-12">                 
                    <p>Add New Admin User: <a href="addAdmin.php" class="btn btn-default">Add User</a></p>
                    <br/>
                </div>
            </div>
            
            <div class="row">
                <div class="col-xs-12">
                    <p class="h4"><strong>List of Current Admin Users</strong></p>
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Username</th>
                                <th>Email</th>
                                <th>Role</th>
                                <th colspan="2"></th>
                            </tr>
                        </thead>
                        
                        <tbody>
                        <?php 
                            if (count($results) > 0) {
                                
                                foreach ($results as $user) {
                                    $userRole = $user["role"];
                                    if ($userRole == "admin") {
                                        $userRole = "Administrator";
                                    } else {
                                        $userRole = "Editor";
                                    }
                                    
                                    if ($login_user == $user["username"]) {
                                        echo '<tr class="info">';
                                    } else {
                                        echo '<tr>';
                                    }
                                    
                                    
                                    echo '<td>'. $user["administratorName"] .'</td>
                                        <td>'. $user["username"] .'</td>
                                        <td>'. $user["administratorEmail"] .'</td>
                                        <td>'. $userRole .'</td>
                                        <td>
                                            <form action="editUser.php" method="post">
                                                <input type="hidden" id="username" name="username" value="' . $user["username"] . '"/>
                                                <input type="submit" id="editButton" name="editButton" class="btn btn-success" value="Edit"/>
                                            </form>
                                        </td>';
                                     
                                    if ($login_user == $user["username"]){
                                        echo '<td></td>';    
                                    } else {
                                        echo '<td>
                                            <form action="process_deleteUser.php" method="post">
                                                <input type="hidden" id ="username" name="username" value="' . $user["username"] . '"/>
                                                <input type="submit" id="deleteButton" name="deleteButton" class="btn btn-danger" value="Delete" onclick="return confirm(\'Are you sure you want to delete this user?\');"/>           
                                            </form>
                                        </td>';
                                    }
                                    
                                    
                                    echo '</tr>';
                                }
                                
                            }
                        ?>
                        </tbody>
                        
                    </table>
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