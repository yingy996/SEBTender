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
    <body> <!--full page background img -->
        <?php 
        include("header.php");
        include("process_viewResponseByUser.php");

        ?>

        <div class="container">
            <div class="row">
                <div class="col-xs-12">
                    <h1>View Survey</h1>
                    <hr/>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12">
                    <p class="h3"> 
                        <strong>
                            <?php
                            if (isset($survey)) {
                                foreach ($survey as $title)
                                    echo $title[0];
                            } else {
                                echo 'Error retreive question title';
                                end;
                            }
                            ?>
                        </strong>
                    </p> 
                    <p class="h4"><strong>Respondents</strong></p>

                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>Respondent ID</th>
                                <th>Date Submitted</th>
                                <th></th>
                            </tr>
                        </thead>

                        <tbody>
                            <?php
                            // Display list of respondents' userID
                            $userEmpty = true;

                            if (count($users) > 0) {

                                foreach ($users as $user) {

                                    echo '<tr>';

                                    echo '<td>'. $user["userID"] .'</td>
                                            <td>'. $user["dateSubmitted"] .'</td>
                                            <td>
                                                <form action="viewUserResponse.php" method="get">
                                                    <input type="hidden" id="surveyID" name="surveyID" value="' . $_GET['surveyID'] . '"/>
                                                    <input type="hidden" id="userID" name="userID" value="' . $user["userID"] . '"/>
                                                    <button type="submit" class="btn btn-info">View Response</button>
                                                </form>
                                            </td>';

                                    $userEmpty = false;

                                    echo '</tr>';
                                }

                            }

                            ?>
                        </tbody>

                    </table>

                    <?php
                    if ($userEmpty == true) 
                        echo '<strong>No survey respondent available.</strong>';
                    ?>


                </div>
            </div>

            <hr/>
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