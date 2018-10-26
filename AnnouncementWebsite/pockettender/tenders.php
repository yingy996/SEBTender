<!DOCTYPE html>
<html data-ng-app="">
<head>
    <title>Pocket Tender | View Tenders</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initialscale=1.0"/>
    <!-- Bootstrap -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/stylesheet.css" rel="stylesheet" />
    
</head>
<body background="../images/paintimg.png"> <!--Photo by rawpixel.com from Pexels -->

    <?php 
    include("header.php");
    include("process_tenders.php");
    ?>
       
    <div class="container-fluid">
        <!--<div class="row">
            <div class="col-xs-12">
                <div class="jumbotron" style="background-color:rgba(255, 255, 255, 0.6)">
                    <div class="row">
                        <div class="col-xs-12 col-sm-6">
                            <h2>Pocket Tender</h2>
                            <hr/>
                            <h5 align="justify" style="line-height:1.6;">Welcome to Pocket Tender! An application that helps you in searching for tenders or procurements in Malaysia! Make tender searching easier now with Pocket Tender. Download the application to explore the awesome features offered!</h5>
                            <br/>

                            <p class="text-center"><a href="file/pockettender.apk" download><img src="../images/downloadBtnlower2.png" alt="Download Button"/></a></p>
                        </div>
                        
                        <div class="clearfix visible-xs"></div>
                        
                        <div class="col-xs-12 col-sm-6 text-center">
                            <img src="../images/pockettenderimg.png" alt="Pocket Tender main page" width="60%">
                        </div>
                    </div>
                      
                </div>
            </div>
        </div>-->
        
        <div class="row">
            <div class="col-xs-12" style="background-color:rgba(255, 255, 255, 0.7)">
                <div class="page-header">
                  <h3>View Tenders</h3>
                </div>
                <!--onclick="window.location=\'tenderDetails.php?ref='. $tender["reference"].'\';" -->
                <?php
                if(count($results) > 0) {
                    foreach($results as $key => $tender) {
                        //echo json_encode($tender);
                        $tenderJson = json_encode($tender);
                        echo '<div class="row" style="background-color:rgba(255, 255, 255, 0.7);padding:20px;cursor: pointer;" data-toggle="modal" data-target="#detailsModal' . $tender["tenderSource"] . '" data-tender="'. $key .'">';
                        echo 
                        '<div class="col-xs-12">
                            <div class="row">
                                <div class="col-xs-12 text-center">
                                    <p><strong>
                                    '. $tender["originatingSource"] .'
                                    </strong></p>
                                </div>
                            </div>';
                            
                        if ($tender["originatingSource"] != "Telekom") {
                            echo '<div class="row">
                                <div class="col-xs-5 text-right">
                                    <p><strong>Reference:</strong>
                                    '. $tender["reference"] .'
                                    </p>
                                </div>                          
                                
                                <div class="col-xs-5 col-xs-offset-2">
                                    <p><strong>Closing Date:</strong>
                                    '. $tender["closingDate"] .'
                                    </p>
                                </div>
                            </div>';
                        }
                            
                            echo '<hr/><div class="row">
                                <div class="col-xs-12 text-center">
                                    <p><strong>Title:</strong></p>
                                    <p>
                                    '. $tender["title"] .'
                                    </p>
                                </div>
                            </div>
                            <hr/>';
                            
                        if ($tender["originatingSource"] != "Telekom"){
                            echo '<div class="row">
                                <div class="col-xs-12 col-sm-5 text-right">';
                                    if (isset($tender["agency"])) {
                                        echo '<p><strong>Agency:</strong> '. $tender["agency"] .'</p>';
                                    }
                            echo '</div><div class="clearfix visible-xs"></div>                          
                                <div class="col-xs-12 col-sm-5 col-sm-offset-2">';
                                    if (isset($tender["category"])) {
                                        echo '<p><strong>Category:</strong>
                                        '. $tender["category"] .'
                                        </p>';
                                    }
                                    
                            echo '</div>
                            </div>';
                        }
                        
                        echo '<div class="row">
                                <div class="col-xs-12 text-center">
                                     <img src="../images/bookmark.png" alt="Bookmark" onclick="alert(\'Bookmark me\');event.stopPropagation();"/>
                                </div>
                            </div>';
                        
                        echo '</div></div><br/>';
                    }
                }
                ?>
            </div>
        </div>
        
        <!-- myProcurement: Modal to display the details of the tender when a tender is clicked -->
        <div class="modal fade" id="detailsModal1" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title text-info" id="detailsModal">Tender details</h4>
                    </div>
                    
                    <div id="modalBody" class="modal-body">
                        <p id="referencep"><strong>Reference:</strong> <span id="reference"></span></p>
                        <hr/>
                        <p id="titlep"><strong>Title:</strong> <span id="title"></span></p>
                        <hr/>
                        <p id="orgSourcep"><strong>Originating Source:</strong> <span id="originatingSource"></span></p>
                        <p id="closingDatep"><strong>Closing Date:</strong> <span id="closingDate"></span></p>
                        <p id="categoryp"><strong>Category:</strong> <span id="category"></span></p>
                        <p id="agencyp"><strong>Agency:</strong> <span id="agency"></span></p>
                    </div>
                </div>
            </div>
        </div>
        
        <!--SEB: Modal to display the details of the tender when a tender is clicked -->
        <div class="modal fade" id="detailsModal0" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title text-info" id="detailsModal">Tender details</h4>
                    </div>
                    
                    <div id="modalBody" class="modal-body">
                        <p id="referencep"><strong>Reference:</strong> <span id="reference"></span></p>
                        <hr/>
                        <p id="titlep"><strong>Title:</strong> <span id="title"></span></p>
                        <hr/>
                        <p id="orgSourcep"><strong>Originating Source:</strong> <span id="originatingSource"></span></p>
                        <p id="closingDatep"><strong>Closing Date:</strong> <span id="closingDate"></span></p>
                        <div id="documentInfoDiv"><hr/><p><strong>Document Information</strong></p>
                            <div id="documentInfo"></div>
                        </div>
                        
                        <div id="originatorInfoDiv"><hr/><p><strong>Originator Information</strong></p>
                            <div id="originatorInfo"></div>
                        </div>
                        <div id="filep"><hr/><strong>Downloadable Files:</strong>
                            <div id="fileLinks"></div>
                        </div>
                        
                    </div>
                </div>
            </div>
        </div>
        
        <!--Telekom: Modal to display the details of the tender when a tender is clicked -->
        <div class="modal fade" id="detailsModal2" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title text-info" id="detailsModal">Tender details</h4>
                    </div>
                    
                    <div id="modalBody" class="modal-body">
                        <p id="referencep"><strong>Reference:</strong> <span id="reference"></span></p>
                        <hr/>
                        <p id="titlep"><strong>Title:</strong> <span id="title"></span></p>
                        <hr/>
                        <p id="orgSourcep"><strong>Originating Source:</strong> <span id="originatingSource"></span></p>
                        <div id="filep"><hr/><strong>Downloadable Files:</strong>
                            <div id="fileLinks"></div>
                        </div>
                        
                    </div>
                </div>
            </div>
        </div>
        
        <!--MBKS: Modal to display the details of the tender when a tender is clicked -->
        <div class="modal fade" id="detailsModal3" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title text-info" id="detailsModal">Tender details</h4>
                    </div>
                    
                    <div id="modalBody" class="modal-body">
                        <p id="referencep"><strong>Reference:</strong> <span id="reference"></span></p>
                        <hr/>
                        <p id="titlep"><strong>Title:</strong> <span id="title"></span></p>
                        <hr/>
                        <p id="orgSourcep"><strong>Originating Source:</strong> <span id="originatingSource"></span></p>
                        <p id="closingDatep"><strong>Closing Date:</strong> <span id="closingDate"></span></p>

                        <div id="filep"><hr/><strong>Downloadable Files:</strong>
                            <div id="fileLinks"></div>
                        </div>
                        
                    </div>
                </div>
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
    
    <!-- Passing data to Bootstrap modal -->
    <script type="text/javascript">
        //SEB modal
        $(document).ready(function(){
            $('#detailsModal0').on('shown.bs.modal', function(e) {
                var tenderKey = $(e.relatedTarget).data('tender');
                var tendersJson = <?php echo json_encode($results); ?>;
                var selectedTender = tendersJson[tenderKey];

                var modal = $(this);
                modal.find('#reference').html(selectedTender.reference);
                modal.find('#title').html(selectedTender.title);
                modal.find('#originatingSource').html(selectedTender.originatingSource);
                modal.find('#closingDate').html(selectedTender.closingDate);
                
                var files = JSON.parse(selectedTender.fileLinks);

                var fileElementsString = '';
                for(var property in files) {
                    if (files.hasOwnProperty(property)){
                        fileElementsString += '<p><a href="' + files[property] + '" target="filetab">' + property + '</a></p>';
                    }
                }
                modal.find('#fileLinks').html(fileElementsString);
                
                var originatorInfo = JSON.parse(selectedTender.originatorJson);
                var originatorElementsString = '';
                var infoTags = [];
                infoTags["name"] = "Name";
                infoTags["officePhone"] = "Office Phone";
                infoTags["extension"] = "Extension";
                infoTags["mobilePhone"] = "Mobile Phone";
                infoTags["email"] = "Email";
                infoTags["fax"] = "Fax";
                
                for(var property in originatorInfo) {
                    if (originatorInfo.hasOwnProperty(property)){
                        if (originatorInfo[property] != null){
                            originatorElementsString += '<p><strong>' + infoTags[property] + ':</strong> ' + originatorInfo[property] + '</p>';
                        }
                    }
                }
                
                modal.find('#originatorInfo').html(originatorElementsString);
                
                var docInfo = JSON.parse(selectedTender.docInfoJson);
                var docElementsString = '';
                var docInfoTags = [];
                docInfoTags["bidCloseDate"] = "Bid Closing Date";
                docInfoTags["feeBeforeGST"] = "Fee before GST";
                docInfoTags["feeGST"] = "GST fee";
                docInfoTags["feeAfterGST"] = "Fee after GST";
                
                for(var property in docInfo) {
                    if (docInfo.hasOwnProperty(property)){
                        if (docInfo[property] != null){
                            docElementsString += '<p><strong>' + docInfoTags[property] + ':</strong> ' + docInfo[property] + '</p>';
                        }
                    }
                }
                modal.find('#documentInfo').html(docElementsString);
            })
        });
        
        //myProcurement modal
        $(document).ready(function(){
            $('#detailsModal1').on('shown.bs.modal', function(e) {
                var tenderKey = $(e.relatedTarget).data('tender');
                var tendersJson = <?php echo json_encode($results); ?>;
                var selectedTender = tendersJson[tenderKey];

                var modal = $(this);
                modal.find('#reference').html(selectedTender.reference);
                modal.find('#title').html(selectedTender.title);
                modal.find('#originatingSource').html(selectedTender.originatingSource);
                modal.find('#closingDate').html(selectedTender.closingDate);
                modal.find('#category').html(selectedTender.category);
                modal.find('#agency').html(selectedTender.agency);
            })
        });
        
        //Telekom modal
        $(document).ready(function(){
            $('#detailsModal2').on('shown.bs.modal', function(e) {
                var tenderKey = $(e.relatedTarget).data('tender');
                var tendersJson = <?php echo json_encode($results); ?>;
                var selectedTender = tendersJson[tenderKey];

                var modal = $(this);
                modal.find('#reference').html(selectedTender.reference);
                modal.find('#title').html(selectedTender.title);
                modal.find('#originatingSource').html(selectedTender.originatingSource);;
                
                var files = JSON.parse(selectedTender.fileLinks);

                var fileElementsString = '';
                for(var property in files) {
                    if (files.hasOwnProperty(property)){
                        fileElementsString += '<p><a href="' + files[property] + '" target="filetab">' + property + '</a></p>';
                    }
                }
                modal.find('#fileLinks').html(fileElementsString);
                
            })
        });
        
        //MBKS modal
        $(document).ready(function(){
            $('#detailsModal3').on('shown.bs.modal', function(e) {
                var tenderKey = $(e.relatedTarget).data('tender');
                var tendersJson = <?php echo json_encode($results); ?>;
                var selectedTender = tendersJson[tenderKey];

                var modal = $(this);
                modal.find('#reference').html(selectedTender.reference);
                modal.find('#title').html(selectedTender.title);
                modal.find('#originatingSource').html(selectedTender.originatingSource);
                modal.find('#closingDate').html(selectedTender.closingDate);
                
                var files = JSON.parse(selectedTender.fileLinks);

                var fileElementsString = '';
                for(var property in files) {
                    if (files.hasOwnProperty(property)){
                        fileElementsString += '<p><a href="http://www.mbks.gov.my' + files[property] + '" target="filetab">' + property + '</a></p>';
                    }
                }
                modal.find('#fileLinks').html(fileElementsString);
            })
        });
    </script>    
</body>
</html>