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
        include("process_viewSurvey.php");

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
                    <h1>View Survey</h1>
                    <hr/>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12">
                    <p class="h4"><strong>Currently Active Surveys</strong></p>
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th style="width: 150px;">Title</th>
                                <th style="width: 600px;">Description</th>
                                <th style="width: 120px;">Published By</th>
                                <th>Start Date</th>
                                <th>End Date</th>
                                <th></th>
                            </tr>
                        </thead>

                        <tbody>
                            <?php 
                            if (count($results) > 0) {

                                $openSurveyEmpty = true;
                                
                                foreach ($results as $survey) {

                                    echo '<tr class="info">';
                                    
                                    if ($survey["isEnded"] == 0)
                                    {
                                        echo '<td>'. $survey["surveyTitle"] .'</td>
                                        <td>'. $survey["description"].'</td>
                                        <td>'. $survey["publishedBy"] .'</td>
                                        <td>'. $survey["startDate"] .'</td>
                                        <td>'. $survey["endDate"] .'</td>
                                        <td>
                                            <form action="survey_get.php" method="get">
                                                <input type="hidden" id="surveyID" name="surveyID" value="' . $survey["surveyID"] . '"/>
                                                <button type="submit" class="btn btn-info">View</button>
                                            </form>
                                        </td>';
                                        
                                        $openSurveyEmpty = false;
                                    }
                                    
                                    echo '</tr>';
                                }

                            }
                            ?>
                        </tbody>

                    </table>

                    <?php
                    if ($openSurveyEmpty == true) 
                        echo '<strong>No survey found.</strong>';
                    ?>

                </div>
            </div>

            <hr/><br/><br/>
            
            <div class="row">
                <div class="col-xs-12">
                    <p class="h4"><strong>Closed Surveys</strong></p>
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th style="width: 150px;">Title</th>
                                <th style="width: 600px;">Description</th>
                                <th style="width: 120px;">Published By</th>
                                <th>Start Date</th>
                                <th>End Date</th>
                                <th></th>
                            </tr>
                        </thead>

                        <tbody>
                            <?php 
                            if (count($results) > 0) {

                                $closedSurveyEmpty = true;

                                foreach ($results as $survey) {

                                    echo '<tr class="info">';

                                    if ($survey["isEnded"] == 1)
                                    {
                                        echo '<td>'. $survey["surveyTitle"] .'</td>
                                        <td>'. $survey["description"].'</td>
                                        <td>'. $survey["publishedBy"] .'</td>
                                        <td>'. $survey["startDate"] .'</td>
                                        <td>'. $survey["endDate"] .'</td>
                                        <td>
                                            <form action="survey_get.php" method="get">
                                                <input type="hidden" id="surveyID" name="surveyID" value="' . $survey["surveyID"] . '"/>
                                                <button type="submit" class="btn btn-info">View</button>
                                            </form>
                                        </td>';

                                        $closedSurveyEmpty = false;
                                    }

                                    echo '</tr>';
                                }

                            }
                            ?>
                        </tbody>

                    </table>

                    <?php
                    if ($closedSurveyEmpty == true) 
                        echo '<strong>No survey found.</strong>';
                    ?>

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