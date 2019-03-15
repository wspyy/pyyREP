
        //点击模块后执行
        function clickModul(modulId) {
            if (modulId == '1') {
                if ($("div[id^='divMenu']")[0] != null && $("div[id^='divMenu']")[0].id.split('_')[1] != null) {
                    modulId = $("div[id^='divMenu']")[0].id.split('_')[1];
                }
                else {
                    //没有任何模块直接退出
                    alert("该用户没有任何权限，请联系管理员!");
                    exitSign();
                }
            }
            clickMenu(modulId, 1);
        }
        
        //点击菜单后执行
        function clickMenu(modulId, menuId) {
            var url;

            if (menuId == '1') {
                menuId = $("a[id^='" + modulId + "_']").first()[0].id.replace(modulId + "_", "");
            }

            //将点击的模块保存到session中

            var data = { menuId: menuId };
            $.post("../Data/UserLogin.ashx", data, function (msg) { });

            url = $("#" + modulId + "_" + menuId)[0].hreflang;
           
            if(url.split('|')[0] != '')
            {
                ShowIndexFrame(url.split('|')[0]);
            }
            
            SetMainFrame(url.split('|')[1]);

        }
        function exitSign() {
            location.href = "../Index.aspx";
        }

        function ShowUserInfo()
        {
            ShowDataFrame('RightsManagement/UserInformation.aspx','600px','300px','用户信息维护',true);
        }
        