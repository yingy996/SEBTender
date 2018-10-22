<!DOCTYPE html>
<html lang="en">
    <head>
        <title>Pocket Tender - Browse</title>
        <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
        <!-- Bootstrap -->
        <link href="pigeon-table/css/bootstrap.min.css" rel="stylesheet" />
        <link href="css/stylesheet.css" rel="stylesheet" />
        <!-- Pigeon Table -->
        <link href="pigeon-table/css/pigeon-table.css" rel="stylesheet" />
    </head>  
    <body> <!--Photo by rawpixel.com from Pexels -->

        <nav class="navbar navbar-inverse">
            <div class="container-fluid">
                <div class="navbar-header">
                    <a class="navbar-brand" href="index.php">Pocket Tender</a>
                </div>
            </div>
        </nav>

        <div class="browse">

            <pigeon-table query="SELECT reference as &#34ReferenceNo&#34, title as Title, category as Category, originatingSource as Originating_Source, agency as Agency, closingDate as Closing_Date, startDate as Start_Date FROM scrapped_tender" editable="false" control="true"></pigeon-table>

        </div>


        <!-- jQuery – required for Bootstrap's JavaScript plugins) -->
        <script src="pigeon-table/js/jquery.min.js"></script>
        <!-- All Bootstrap plug-ins file -->
        <script src="pigeon-table/js/bootstrap.min.js"></script>
        <!-- AngularJS -->
        <script src="pigeon-table/js/angular.min.js"></script>
        <!-- Angular UI Bootstrap -->
        <script src="pigeon-table/js/ui-bootstrap-tpls-2.5.0.min.js"></script>
        <!-- Pigeon Table -->
        <script src="pigeon-table/js/pigeon-table.js"></script> 

    </body>
</html>
