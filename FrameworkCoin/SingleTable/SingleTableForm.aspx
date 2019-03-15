<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SingleTableForm.aspx.cs"
    Inherits="SingleTable_SingleTableForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FormData</title>
    <style type="text/css">
        .error {
            color:red;
        }
    </style>
</head>
<body>
    <div class="row">
        <div class="col-xs-12">
            <div class="box">
                <div class="box-header">
                    <div class="box-name">
                        <%=strManageTitle %>
                    </div>
                    <div class="box-icons">
                        <a class="collapse-link">
                            <i class="fa fa-chevron-up"></i>
                        </a>
                        <a class="expand-link">
                            <i class="fa fa-expand"></i>
                        </a>
                        <a class="close-link">
                            <i class="fa fa-times"></i>
                        </a>
                    </div>
                    <div class="no-move"></div>
                </div>
                <div class="box-content">

                    <!-- BEGIN FORM-->
                    <form class="form-horizontal form-bordered form-row-strippedl" id="formManage" runat="server">
                        <%=strManageButton %>
                        <%=strManageBody %>
                    </form>
                    <!-- END FORM-->
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        
    </script>
</body>
</html>
