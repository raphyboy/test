<%@ Page Language="VB" MasterPageFile="~/MasterPageMenu.master" AutoEventWireup="false" CodeFile="edit_service.aspx.vb" Inherits="edit_service" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

     <div class="col-md-10">

            <ol class="breadcrumb headingbar">
            <li class="breadcrumb-item"><a id="menu_capex" runat="server">Edit Capex</a></li>
            <li class="breadcrumb-item"><a id="menu_opex" runat="server">Edit Opex</a></li>
            <li class="breadcrumb-item active">Edit Service</li>
            <li class="breadcrumb-item"><a id="menu_summary" runat="server">Edit Summary</a></li>
            </ol>
            
            <div class="container">
                <div class="form-horizontal">
                    <div class="form-group">
                         <label class="col-md-2 control-label">อายุสัญญา Contract :</label>  
                         <div class="col-md-2">
                             <div class="input-group">
                             <asp:TextBox ID="txtContract" runat="server" CssClass="form-control"></asp:TextBox>
                             <span class="input-group-addon">Month</span>
                             </div>
                         </div>
                    </div>      
                    <div class="form-group">
                        <label class="col-md-2 control-label" style="text-decoration: underline;">Discount</label> 
                        <div class="col-md-2">
                        <asp:DropDownList ID="ddlDiscount" runat="server" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    
                    <hr style="border-color: #DDD;" />
                 <div class="col-md-12">   
                  <div class="col-md-5"> 
                    <div class="form-group">
                        <div class="col-md-12">
                        <label class="control-label" style="text-decoration: underline;">INL Service</label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-5 control-label">Domestic</label> 
                        <div class="col-md-5">
                            <div class="input-group">
                            <asp:TextBox ID="txtDom" runat="server" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon">Mbps</span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-5 control-label">Dom.Utilization</label>	
                        <asp:Label ID="lblDom" runat="server" CssClass="col-md-1 control-label"></asp:Label>
                        <label class="control-label">%</label>
                    </div>
                    <div class="form-group">
                        <label class="col-md-5 control-label">International</label> 
                        <div class="col-md-5">
                            <div class="input-group">              
                            <asp:TextBox ID="txtInter" runat="server" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon">Mbps</span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-5 control-label">Inter.Utilization</label>	
                        <asp:Label ID="lblInterUtilization" runat="server" CssClass="col-md-1 control-label"></asp:Label>
                        <label class="control-label">%</label>
                    </div>
                    <div class="form-group">
                        <label class="col-md-5 control-label">Transit</label> 
                        <div class="col-md-5">
                            <div class="input-group">  
                            <asp:TextBox ID="txtTransit" runat="server" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon">Mbps</span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-5 control-label">Total INL Curcuits</label> 
                        <div class="col-md-5">
                            <div class="input-group">  
                            <asp:TextBox ID="txtTotalINL" runat="server" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon">Curcuits</span>
                            </div>
                        </div>
                    </div>   
                 </div>
                 
                     <div class="col-md-5">
                        <div class="form-group">
                            <div class="col-md-12">
                            <label class="control-label" style="text-decoration: underline;">IPV Service</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-5 control-label">Ethernet</label> 
                            <div class="col-md-5">
                                <div class="input-group"> 
                                <asp:TextBox ID="txtEthernetIPV" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon">Mbps</span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-5 control-label">IPV.Utilization</label>	
                            <asp:Label ID="lblIPVUtilization" runat="server" class="col-md-1 control-label"></asp:Label>
                            <label class="control-label">%</label>
                        </div>
                        <div class="form-group">
                            <label class="col-md-5 control-label">Total IPV Curcuits</label> 
                            <div class="col-md-5">
                                <div class="input-group">  
                                <asp:TextBox ID="txtTotalIPV" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon">Curcuits</span>
                                </div>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <div class="col-md-12">
                            <label class="control-label" style="text-decoration: underline;">INP Service</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-5 control-label">Ethernet</label> 
                            <div class="col-md-5">
                                <div class="input-group"> 
                                <asp:TextBox ID="txtEthernetINP" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon">Mbps</span>
                                </div>
                            </div>
                        </div>  
                        <div class="form-group">
                            <label class="col-md-5 control-label"></label> 
                            <div class="col-md-7">
                                <div class="input-group"> 
                                <asp:TextBox ID="txtPriceINP" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon">THB (exVAT)</span>
                                </div>
                            </div>
                        </div> 
                        <div class="form-group">
                            <label class="col-md-5 control-label">Total INP Curcuits</label> 
                            <div class="col-md-5">
                                <div class="input-group">  
                                <asp:TextBox ID="txtTotalINP" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon">Curcuits</span>
                                </div>
                            </div>
                        </div>
                       <div class="form-group">
                        <div class="col-md-12">
                        <label class="control-label" style="text-decoration: underline;">Selling Price (Ex.VAT)</label>
                        </div>
                    </div> 
                    <div class="form-group">
                        <label class="col-md-5 control-label">Monthly (รายได้)</label> 
                        <div class="col-md-7">
                            <div class="input-group">  
                            <asp:TextBox ID="txtMonthly" runat="server" CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">THB (exVAT)</span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-5 control-label">Total Yearly</label>	
                        <asp:Label ID="lblTotalYealy" runat="server" Text="Label" class="col-md-2 control-label"></asp:Label>
                    	<label class="control-label" style="font-weight: 100;">THB (exVAT)</label>
                    </div>
                    <div class="form-group">
                        <label class="col-md-5 control-label">One-Time Payment</label> 
                        <div class="col-md-7">
                            <div class="input-group">  
                            <asp:TextBox ID="txtOneTime" runat="server" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon">THB (exVAT)</span>
                            </div>
                        </div>
                    </div>  
                 </div>
                 <div class="col-md-12">
                    <asp:Button ID="Button1" runat="server" Text="บันทึก" class="btn btn-success pull-right"/> 
                 </div>    
                 </div>

                    
                    <!--<div class="col-md-12 hidden" >
            
                        <div class="panel panel-info">
                            <div class="panel-body">
                            Revenue : <asp:Label ID="lblRevenue" runat="server"></asp:Label>
                            <div class="clearfix"></div>
                            One-Time Payment : <asp:Label ID="lblOneTime" runat="server"></asp:Label>
                            <div class="clearfix"></div>
                            Monthly Payment : <asp:Label ID="lblMonthly" runat="server"></asp:Label>
                            <div class="clearfix"></div>
                            OPEX
                            <div class="clearfix"></div>
                                <div class="col-md-12">
                                MKT Cost 3% <asp:Label ID="lblMKTCost" runat="server"></asp:Label>
                                <div class="clearfix"></div>
                                Cost of Internet Bandwidth : <asp:Label ID="lblCostOfInternet" runat="server"></asp:Label>
                                <div class="clearfix"></div>
                                Domestic Cost : <asp:Label ID="lblDomesticCost" runat="server"></asp:Label>
                                <div class="clearfix"></div>
                                Internatonal Cost : <asp:Label ID="lblInternationalCost" runat="server"></asp:Label>
                                <div class="clearfix"></div>
                                Transit : <asp:Label ID="lblTransitCost" runat="server"></asp:Label>
                                <div class="clearfix"></div>
                                xDSL, FTTx : <asp:Label ID="lblServiceCost" runat="server"></asp:Label>
                                <div class="clearfix"></div>
                                Cost of Network Bandwidth : <asp:Label ID="lblCostOfNetwork" runat="server"></asp:Label>
                                <div class="clearfix"></div>
                                Cost of NOC : <asp:Label ID="lblCostOfNOC" runat="server"></asp:Label>
                                <div class="clearfix"></div>
                                VAS & COST per month : <asp:Label ID="lblVas" runat="server"></asp:Label>
                                <div class="clearfix"></div>
                                </div>
                            Revenue after Operation : <asp:Label ID="lblRevenueAfter" runat="server"></asp:Label>
                            <div class="clearfix"></div>    
                            CAPEX (Include Spare Part) : <asp:Label ID="lblCAPEX" runat="server"></asp:Label>
                            <div class="clearfix"></div>
                            </div>
                        </div>
                    </div>-->
                 
             </div>
             
          </div>
          
         
       </div>

        
</asp:Content>  
