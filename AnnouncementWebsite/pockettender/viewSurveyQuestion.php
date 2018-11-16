<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>Pocket Tender</title>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initialscale=1.0"/>
        <!-- Bootstrap -->
        <link href="../css/bootstrap.min.css" rel="stylesheet" />
        <link href="../css/stylesheet.css" rel="stylesheet" />

    </head>
    <body> <!--full page background img -->
        <?php 
        include("header.php");
        include("process_viewSurveyQuestion.php");

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
                                echo 'Error retreive survey title';
                                end;
                            }
                            ?>
                        </strong>
                    </p> <br/>
                    <p class="h4"><strong>Survey Question List</strong></p>

                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>No.</th>
                                <th>Title</th>
                                <th>Type</th>
                                <th></th>
                            </tr>
                        </thead>

                        <tbody>
                            <?php 
                            if (count($questions) > 0) {

                                foreach ($questions as $question) {

                                    echo '<tr>';
                                    
                                    $qNum = (int)$question["questionNumber"] + 1;
                                    
                                    echo 
                                        '<td>'. $qNum .'</td>
                                        <td>'. $question["questionTitle"].'</td>
                                        <td>'. $question["questionType"].'</td>
                                        <td>
                                            <form action="viewQuestionResponse.php" method="get">
                                                <input type="hidden" id="questionType" name="questionType" value="' . $question["questionType"] . '"/>
                                                <input type="hidden" id="surveyID" name="surveyID" value="' . $_GET['surveyID'] . '"/>
                                                <input type="hidden" id="questionID" name="questionID" value="' . $question["questionID"] . '"/>
                                                <button type="submit" class="btn btn-info btn-block">View Response</button>
                                            </form>
                                        </td>';


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
        <script src="../js/jquery.min.js"></script>
        <!-- All Bootstrap plug-ins file -->
        <script src="../js/bootstrap.min.js"></script>
        <!--Basic AngularJS-->

    </body>
</html>