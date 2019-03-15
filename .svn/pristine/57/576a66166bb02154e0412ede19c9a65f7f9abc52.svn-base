<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SingleTableGrid.aspx.cs"
    Inherits="SingleTable_SingleTableGrid" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GridData</title>
</head>
<body>
    <div class="row">
        <div class="col-xs-12">
            <!-- BEGIN EXAMPLE TABLE PORTLET-->
            <div class="box">
                <div class="box-header">
                    <div class="box-name">
                        <%=strGridTitle %>
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
                    <%=strGridButton %>
                    <%=strGridBody %>
                    <ul id='boot_pagination'>
                    </ul>
                    <ul class="pagination pull-right">
                        <li class="disabled"><a href="javascript:void(0)">
                            <%=strPageInfo%></a></li>
                    </ul>
                </div>
            </div>
            <!-- END EXAMPLE TABLE PORTLET-->
        </div>
    </div>

    <script src="plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/bootstrap-paginator.js" type="text/javascript"></script>
    <script type="text/javascript">

        $("#dataTable tbody tr [type='checkbox']").change(function(){
            if($(this)[0].checked)
            {
                $(this).parents('span').removeClass().addClass("checked");
                $(this).parents('tr').removeClass().addClass("active");
            } else {
                $(this).parents('span').removeClass();
                $(this).parents('tr').removeClass();
            }
        });
 
        var element = $('#boot_pagination');

        var options = {
            bootstrapMajorVersion: 3,
            currentPage: args["pageNum"],
            numberOfPages: 10,
            totalPages: <%=strAllPageNum %>,
            useBootstrapTooltip:true,
            tooltipTitles: function (type, page, current) {
                switch (type) {
                case "first":
                    return "跳转首页 <i class='icon-fast-backward icon-white'></i>";
                case "prev":
                    return "前一页 <i class='icon-backward icon-white'></i>";
                case "next":
                    return "后一页 <i class='icon-forward icon-white'></i>";
                case "last":
                    return "跳转尾页 <i class='icon-fast-forward icon-white'></i>";
                case "page":
                    return "跳转 " + page + " 页<i class='icon-file icon-white'></i>";
                }
            },
            onPageClicked: function(e,originalEvent,type,page){
                args["pageNum"] = page;
                refreshGrid();
            }
        }
        element.bootstrapPaginator(options);
    </script>
</body>
</html>
