<%@ Page Language="VB" MasterPageFile="~/MasterPageMenu.master" AutoEventWireup="false" CodeFile="Summary.aspx.vb" Inherits="Summary" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!--<link rel="stylesheet" type="text/css" href="App_Inc/jquery-ui-1.11.4/jquery-ui.css" />-->
    <div class="col-md-10 pull-right">   
            
            <ol class="breadcrumb headingbar">
            <li class="breadcrumb-item"><a href="Default.aspx">Default</a></li>
            <li class="breadcrumb-item"><a href="add_opex.aspx">Add Opex</a></li>
            <li class="breadcrumb-item"><a href="add_service.aspx">Add Service</a>
            </li><li class="breadcrumb-item active">Summary</li></ol>
            

            <div class="col-md-12" style="border: 1px solid black; padding-bottom: 15px; margin: 10px 0px 20px 0px;"> 
                
                    <div class="col-md-12" style="border: 1px solid black; padding-bottom: 15px; margin: 10px 0px 0px 0px;"> 

                            <asp:Label ID="Label2" runat="server" Text="Part ......" CssClass="pull-right"></asp:Label>
                            <div class="col-md-12" style="text-align : center;">
                            <asp:Label ID="Label1" runat="server" Text="แบบอนุมัติแผนงานให้บริการอินเตอร์เน็ตไร้สาย CyberPoint" Font-Bold="True"></asp:Label>
                            </div>
                            
                    </div> 
                    
                    <div class="col-md-12" style="padding-left: 0px;border-left: 1px solid black;border-right: 1px solid black;border-bottom: 1px solid black;"> 
                    
                            <div class="col-md-5" style="padding-left: 0px;border-right: 1px solid black;">
                            <asp:Label ID="Label3" runat="server" Text="Ref. No. :"></asp:Label>
                            <asp:Label ID="lblDocumentNo" runat="server"></asp:Label>
                            </div>
                            
                            <div class="col-md-5" style="padding-left: 0px;border-right: 1px solid black;">
                            <asp:Label ID="Label5" runat="server" Text="วันที่จัดทำ/ปรับปรุง :"></asp:Label>
                    <%--      <asp:TextBox ID="txtDocumentDate" runat="server" type="date"></asp:TextBox>--%>
                            <input id="txtDocumentDate" runat="server" type="text" data-field="date"/>
                            <div id="dtBox"></div>
                            </div>
                            
                             <div class="col-md-2" style="padding-left: 0px;">
                             <asp:Label ID="Label7" runat="server" Text=" Area :"></asp:Label>
                             <asp:DropDownList ID="ddlArea" runat="server" Width="70px" AutoPostBack="True"></asp:DropDownList>
                             </div>
                        </div>  
                    
                    <div class="col-md-12" style="padding-left: 0px;border-left: 1px solid black;border-right: 1px solid black;border-bottom: 1px solid black;"> 
                            
                            <div class="col-md-5" style="padding-left: 0px;border-right: 1px solid black;">
                            <asp:Label ID="Label9" runat="server" Text="Ref.LocationName/Code"></asp:Label>
                            <asp:TextBox ID="txtLocationName" runat="server"></asp:TextBox>
                            </div>
                            
                            <div class="col-md-5" style="padding-left: 0px;border-right: 1px solid black;">
                            <asp:Label ID="Label11" runat="server" Text="วันที่เริ่มให้บริการ :"></asp:Label>
                            <%--<asp:TextBox ID="txtServiceDate" runat="server"></asp:TextBox>--%>
                            <input id="txtServiceDate" runat="server" type="text" data-field="date"/>
                            <div id="Div1"></div>
                            </div>
                            
                            <div class="col-md-2" style="padding-left: 0px;">        
                            <asp:Label ID="Label13" runat="server" Text="Cluster :"></asp:Label>
                            <asp:DropDownList ID="ddlCluster" runat="server" Width="70px"></asp:DropDownList>
                            </div>
                            
                     </div>       
                
                    <div class="col-md-12" style="padding-left: 0px; border-left: 1px solid black;border-right: 1px solid black;border-bottom: 1px solid black;"> 
                    
                            <div class="col-md-5" style="padding-left: 0px;border-right: 1px solid black;">
                            <asp:Label ID="Label15" runat="server" Text="Customer Name :"></asp:Label>
                            <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-5" style="padding-left: 0px;border-right: 1px solid black;">
                            </div>
                           
                            <div class="col-md-2">
                            </div>
                    </div>
               
                <div class="col-md-12" style="padding-left: 0px;padding-right: 0px; border-left: 1px solid black;border-right: 1px solid black;border-bottom: 1px solid black;"> 
                
                    <!--<div class="col-md-2" style="padding-left: 0px;background-color:#FFCBA4;min-height: 8.5% !important;text-align: center;padding-top: 15px;">
                    <asp:Label ID="Label17" runat="server" Text="Type of Service"></asp:Label>
                    </div>-->
                    
                    <div class="col-md-10" style="padding-left: 0px;padding-right: 0px; background-color: #B9FFA3;">
                        <div class="col-md-3">
                        <asp:RadioButton ID="RadioButton1" runat="server" Checked="True" Text="New Service" GroupName="type" />
                        <br />
                        <!--<asp:RadioButton ID="RadioButton5" runat="server" Text="New Boundary" GroupName="type" />-->
                        </div>
                        
                        <div class="col-md-3">
                        <asp:RadioButton ID="RadioButton2" runat="server" GroupName="type" Text="Re-New Service" />
                        <br />
                        <!--<asp:RadioButton ID="RadioButton6" runat="server" Text="Existing Boundary" GroupName="type" />-->
                        </div>
                        
                        <div class="col-md-4">
                        <asp:RadioButton ID="RadioButton3" runat="server" GroupName="type" Text="Maintenance" />
                        <br />
                        <!--<asp:RadioButton ID="RadioButton7" runat="server" Text="Maintenance (Not Revenue)" GroupName="type" />-->
                        </div>
                        
                        <div class="col-md-2" style="padding-right: 0px;">
                        <!--<asp:RadioButton ID="RadioButton4" runat="server" Checked="True" GroupName="type" Text="INL" />-->
                        </div>
                    </div>  
             </div>       

            
            <div class="col-md-12" style="padding-left: 0px;padding-right: 0px;border-left: 1px solid black;border-right: 1px solid black;border-bottom: 1px solid black;"> 
                    <div class="col-md-12">
                    <asp:Label ID="Label18" runat="server" Text="Description ที่ขออนุมัติ"></asp:Label>
                    </div>
                    
                    <div class="col-md-12">
                        <div class="col-md-3">
                        <asp:Label ID="Label19" runat="server" Text="รายโครงการต่อเดือน :"></asp:Label>
                        </div>
                        <div class="col-md-2">
                        <asp:Label ID="lblMonthly" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-md-7">
                        <asp:Label ID="Label21" runat="server" Text="THB (exVAT)"></asp:Label>
                        </div>
                    </div>
                    
                    <div class="col-md-12">
                        <div class="col-md-3">    
                        <asp:Label ID="Label22" runat="server" Text="เงินชำระครั้งเดียว :"></asp:Label>
                        </div>
                        <div class="col-md-2">
                        <asp:Label ID="lblOneTimePayment" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-md-7">
                        <asp:Label ID="Label24" runat="server" Text="THB (exVAT)"></asp:Label>
                        </div>
                    </div>    
                    
                    <div class="col-md-12">
                        <div class="col-md-3">    
                        <asp:Label ID="Label25" runat="server" Text="รายละเอียดการให้บริการ :"></asp:Label>
                        </div>
                        <div class="col-md-9">
                        <asp:Label ID="Label26" runat="server" Text="(DCN+INL)"></asp:Label>
                        </div>
                    </div>
                        
                     <div class="col-md-12">
                        <div class="col-md-3">
                        <asp:Label ID="Label27" runat="server" Text="งานที่ต้องดำเนินการ :"></asp:Label>    
                        </div>
                        <div class="col-md-9" style="background-color:#FFFFC2;">
                        <asp:TextBox ID="txtDetailService" runat="server" TextMode="MultiLine" Height="100px" Width="550px"></asp:TextBox>
                        </div> 
                    </div>  
                                       
                    <div class="col-md-12">
                      <div class="col-md-3">       
                        <asp:Label ID="Label28" runat="server" Text="เงินลงทุน (CAPEX) :"></asp:Label>
                      </div>
                      <div class="col-md-2">      
                        <asp:Label ID="lblCAPEX" runat="server" Text=""></asp:Label> 
                      </div>
                      <div class="col-md-7">     
                        <asp:Label ID="Label30" runat="server" Text="THB (exVAT)"></asp:Label> 
                      </div>
                    </div>   
                    
                    <div class="col-md-12">
                      <div class="col-md-3"> 
                        <asp:Label ID="Label31" runat="server" Text="สัญญา (ปี) :"></asp:Label>  
                      </div>
                      <div class="col-md-2">   
                        <asp:Label ID="lblContractYear" runat="server"></asp:Label>  
                      </div>
                      <div class="col-md-7">   
                        <asp:Label ID="Label32" runat="server" Text="ปี"></asp:Label>
                      </div>
                    </div>
                    
                    <div class="col-md-12" style="background-color:#FFFF99;">
                        <div class="col-md-7" >
                        <asp:Label ID="Label34" runat="server" Text="Investment Cost (CAPEX)"></asp:Label>
                        </div>
                        
                        <div class="col-md-5">
                        <asp:Label ID="Label35" runat="server" Text="VAS & Cost (per Month)"></asp:Label>
                        
                        </div>
                    </div>
                    <div class="col-md-7" >
                    <div id="CAPEX" runat="server"></div>
                    </div>
                    <div class="col-md-5">
                    <div id="OPEX" runat="server"></div>
                    </div>

            </div>
            
             <div class="col-md-12" style="padding-left:0px">
             
                    <asp:Label ID="Label20" runat="server" Text="Cumulative"></asp:Label>
                    <asp:Label ID="lblContract" runat="server"></asp:Label>
                    <asp:Label ID="Label29" runat="server" Text="Months"></asp:Label>
                    <br />
                    
                    <div class="col-md-3" style="padding-left:0px; padding-right:10px;margin-right: 14px;">
                    <asp:Label ID="Label36" runat="server" Text="Revenue"></asp:Label>
                    </div>
                    <asp:Label ID="lblRevenue" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="Label38" runat="server" Text="OPEX"></asp:Label>
                    
                    
                        <div style="color:#696969; margin-left: 20px;"> 
                        
                            <div class="col-md-3">
                            <asp:Label ID="Label39" runat="server" Text="MKT Cost 3%"></asp:Label>
                            </div>
                            <asp:Label ID="lblMKTCost" runat="server" Text=""></asp:Label>
                            <br />
                           
                            <div class="col-md-3">
                            <asp:Label ID="Label40" runat="server" Text="Cost of Internet Bandwidth"></asp:Label>
                            </div>
                            <asp:Label ID="lblCostOfInternet" runat="server" Text=""></asp:Label>
                            <br />
                            
                            <div class="col-md-3">
                            <asp:Label ID="Label41" runat="server" Text="Cost of Network Bandwidth"></asp:Label>
                            </div>
                            <asp:Label ID="lblCostOfNetwork" runat="server" Text=""></asp:Label>
                            <br />
                           
                            <div class="col-md-3">
                            <asp:Label ID="Label42" runat="server" Text="Cost of NOC"></asp:Label>
                            </div>
                            <asp:Label ID="lblCostOfNOC" runat="server" Text=""></asp:Label>
                            <br />
                            
                            <div class="col-md-3">
                            <asp:Label ID="Label43" runat="server" Text="VAS & COST per month"></asp:Label>
                            </div>
                            <asp:Label ID="lblVas" runat="server" Text=""></asp:Label>
                            
                        </div>
                    <div class="col-md-3" style="padding-left:0px; padding-right:10px;margin-right: 14px;">  
                    <asp:Label ID="Label44" runat="server" Text="Cash Flow"></asp:Label>
                    </div>
                    <asp:Label ID="lblCashFlow" runat="server" Text=""></asp:Label>
             </div>   

          <div class="col-md-4" style="border: 1px solid black;"> 
           
                  <div class="col-md-6" style="border-right: 1px solid black;">
                      <asp:Label ID="Label37" runat="server" Text="Payback (months)"></asp:Label>
                      <br />
                      <asp:Label ID="Label33" runat="server" Text="Margin"></asp:Label>
                      <br />
                      <asp:Label ID="Label23" runat="server" Text="NPV (5% / Year)"></asp:Label>                       
                  </div>
                  
                  <div class="col-md-6">
                       <asp:Label ID="lblPayBack" runat="server" Text=""></asp:Label>
                       <br />
                       <asp:Label ID="lblMargin" runat="server"></asp:Label>
                       <br />
                       <asp:Label ID="lblNPV" runat="server"></asp:Label>
                  </div>
           </div>
          
        
           <div class="col-md-12" style="border: 1px solid black; margin-top: 5%; margin-bottom: 2%;"> 
           
             <div class="col-md-4" style="border-right: 1px solid black;padding-left: 0px;"> 
                    
                    <div style="text-align: center;">
                    <asp:Label ID="Label45" runat="server" Text="Prepared by"></asp:Label>
                    </div>
                    <br />
                    <div style="margin-top:50px;text-align: center;">
                    <asp:Label ID="lblPrepare" runat="server"></asp:Label>
                    </div>
                    <br />
                    <asp:Label ID="Label51" runat="server" Text="Date"></asp:Label>
            </div>
            
            <div class="col-md-4" style="border-right: 1px solid black;"> 
            
                    <div style="text-align: center;">
                    <asp:Label ID="Label46" runat="server" Text="Verified by HRO"></asp:Label>
                    </div>
                    <br />
                    <div style="margin-top:50px;text-align: center;">
                    <asp:Label ID="Label49" runat="server" Text="(คุณสิทธา  สุวิรัชวิทยกิจ)"></asp:Label>
                    </div>
                    <br />
                    <asp:Label ID="Label52" runat="server" Text="Date"></asp:Label>
                    
            </div>
                    
            <div class="col-md-4"> 
                    
                    <div style="text-align: center;">
                    <asp:Label ID="Label47" runat="server" Text="Verifired by Network"></asp:Label>
                    </div>
                    <br />
                    <div style="margin-top:50px;text-align: center;">
                    <asp:Label ID="Label50" runat="server" Text="(คุณรังษี  วนเศรษฐ)"></asp:Label>
                    </div>
                    <br />
                    <asp:Label ID="Label53" runat="server" Text="Date"></asp:Label>
                    
            </div>
            
          </div>
   
     
      </div>
        
      <asp:Button ID="btnSave" runat="server" Text="Save" />
      
    
    </div>
<script type="text/javascript">

    $("#dtBox").DateTimePicker({
    dateSeparator: "/",
    dateFormat: "dd/MM/yyyy",
   


    
    });
    
</script>    
</asp:Content>  