<%@ Page Title="Ansatte" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnsattList.aspx.cs" Inherits="Timeplan.Lists.AnsattList" %>

<%--<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--<link type="text/css" href="Content/plugins/jquery.multiselect.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/ui-lightness/jquery-ui.css" />

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="Scripts/plugins/jquery.multiselect.js"></script>--%>

    <%--<link rel="stylesheet" href="../Content/plugins/chosen.min.css">
    <script type="text/javascript" src="../Scripts/plugins/chosen.jquery.min.js"></script>--%>

    

    <%--INFO: too slow, needed better performing multiselect dropdown.--%>

    <%--<script type="text/javascript" src="Scripts/bootstrap.min.js"></script>--%>

    <%--<link type="text/css" href="Content/plugins/checkboxDropdown/bootstrap-multiselect.css" rel="stylesheet" />--%>
    <%--<link type="text/css" href="Content/bootstrap.min.css" rel="stylesheet" />--%>


    <%--<script type="text/javascript" src="Scripts/bootstrap.min.js"></script>--%>

    <%--<script type="text/javascript">

        //var jQuery_1_8_3 = $.noConflict(true);

    </script>--%>
    
    <br />

    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Lagre alle" OnClick="SaveAllButton_Click" ID="SaveButtonTop" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Avbryt" CausesValidation="false" OnClick="CancelButton_Click" ID="CancelButtonTop" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Opprett ny ansatt" OnClick="CreateNewButton_Click" ID="CreateNewButtonTop" />

    <section>
        <div>
            <hgroup>
                <h2><%: Page.Title %></h2>
            </hgroup>

            <asp:HiddenField ID="ScrollPosition" runat="server" Value="0" />

            <asp:ListView
                ID="AnsattListView"
                runat="server"
                ItemPlaceholderID="AnsattItem"
                OnSorting="ListViewEvents_Sorting">

                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tbody>
                            <tr class="listview-heading">
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Id" ID="IdLinkButton" runat="server" Visible="False">Id</asp:LinkButton>
                                    <div class="show-hide">
                                        <%--TODO: better descrition/view of show hide func--%>
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowIdLinkButton" Visible="False"></asp:LinkButton>
                                    </div>
                                </th>
                                <th style="width: 16%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Navn" ID="NavnLinkButton" runat="server">Navn</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowNavnLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th style="width: 5%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Stillingsstørrelse" ID="StillingsStørrelseLinkButton" runat="server">Stilling (%)</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowStillingsStørrelseLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th style="width: 8%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Tlfnr" ID="TlfnrLinkButton" runat="server">Tlfnr</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowTlfnrLinkButton" ToolTip="Vis/Skjul Tlfnr"
                                            
                                            
                                            >

                                            <%--var hidedColumns = (Dictionary<string, string>)Session["AnsattList - hidedColumns"];

                                                title="<%# String.Format("javascript:return ConfirmDelete()") %>"

                                                OnClientClick='<%# String.Format("javascript:return ConfirmDelete(\"{0}\")", Eval("Navn") != null ? Eval("Navn").ToString() : string.Empty) %>' 

                                                title='<%# String.Format("javascript:return ConfirmDelete(\"{0}\")", Eval("Navn") != null ? Eval("Navn").ToString() : string.Empty) %>' 
                                                 if (Session["AnsattList - hidedColumns"] != null) { return "Grrr!!!" } else { return "Artig?" }

                                            if (hidedColumns.ContainsKey(tableHeader))
                                            {
                                                hidedColumns.Remove(tableHeader);
                                            }
                                            else
                                            {
                                                hidedColumns.Add(tableHeader, tableData);
                                            }--%>


                                        </asp:LinkButton>
                                    </div>
                                </th>
                                <th style="width: 13%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Avdeling" ID="AvdelingLinkButton" runat="server">Avdeling</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowAvdelingLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th style="width: 12%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="StillingsType" ID="StillingsTypeLinkButton" runat="server">Stillingstype</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowStillingsTypeLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th style="width: 1%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="JobberIKlasser" ID="JobberIKlasserLinkButton" runat="server">Klasser</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowJobberIKlasserLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th style="width: 5%;">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="JobberISfos" ID="JobberISfosLinkButton" runat="server">Sfo</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowJobberISfosLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <%--<th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="AvdelingsLederIAvdelinger" ID="AvdelingsLederIAvdelingerLinkButton" runat="server">Avdelingsleder i avdelinger</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowAvdelingsLederIAvdelinger"></asp:LinkButton>
                                    </div>
                                </th>--%>
                                <th></th>
                                <th></th>
                                <th></th>
                                <th></th>
                            </tr>
                            <tr id="AnsattItem" runat="server"></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>

                <EmptyItemTemplate>
                    <p>
                        Ingen ansatte funnet
                    </p>
                </EmptyItemTemplate>

                <%--TODO: whats this?--%>
                <%--<GroupTemplate>
                    <tr id="itemPlaceholderContainer" runat="server">
                        <td id="itemPlaceholder" runat="server"></td>
                    </tr>
                </GroupTemplate>--%>

                <ItemTemplate>
                    <tr class="row-standard">
                        <td>
                            <div class="form-group">
                                <asp:Button CssClass="form-control" runat="server" ID="IdButton" Text='<%# Eval("Id") %>' Visible="False"></asp:Button>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control"  runat="server" ID="NavnTextBox" MaxLength="100" Text='<%# Eval("Navn") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RequiredFieldValidator runat="server" ID="NavnRequiredFieldValidator" Display="Dynamic" ControlToValidate="NavnTextBox" ErrorMessage="Påkrevd" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control percentage" runat="server" ID="StillingsStørrelseTextBox" MaxLength="6" Text='<%# Eval("Stillingsstørrelse") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RangeValidator runat="server" ID="StillingsStørrelseRangeValidator" ControlToValidate="StillingsStørrelseTextBox" Type="Double" MinimumValue="0" MaximumValue="100" Display="Dynamic" ErrorMessage="Ugyldig %" />
                                    <asp:RequiredFieldValidator runat="server" ID="StillingsStørrelseRequiredFieldValidator" ControlToValidate="StillingsStørrelseTextBox" Display="Dynamic" ErrorMessage="Påkrevd" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" runat="server" ID="TlfNrTextBox" MaxLength="8" Text='<%# Eval("Tlfnr") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RangeValidator runat="server" ID="TlfnrRangeValidator" ControlToValidate="TlfNrTextBox" Type="Double" MinimumValue="10000000" MaximumValue="99999999" Display="Dynamic" ErrorMessage="Ugyldig nr" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="AvdelingDropDown"></asp:DropDownList>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="StillingsTypeDropDown"></asp:DropDownList>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:CheckBoxList CssClass="form-control scrollingControlContainer" runat="server" Visible="True" ID="JobberIKlasserListBox" Height="34"></asp:CheckBoxList>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <%--<asp:DropDownCheckBoxes ID="DropDownCheckBoxes1" runat="server"></asp:DropDownCheckBoxes>--%>
                                <asp:CheckBoxList CssClass="form-control scrollingControlContainer" runat="server" Visible="True" ID="JobberISfosListBox" Height="34"></asp:CheckBoxList>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <%--<td>
                            <div class="form-group">
                                <asp:ListBox CssClass="form-control" runat="server" SelectionMode="Multiple" Visible="True" ID="AvdelingsLederIAvdelingerListBox" Height="1"></asp:ListBox>
                            </div>
                        </td>--%>
                        <td>
                            <div class="form-group btn-save">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="SaveButton_Click" ID="SaveLinkButton" ToolTip="Lagre endringer">
                                    <span class="glyphicon glyphicon-floppy-saved"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group btn-delete">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" 
                                    OnClientClick='<%# String.Format("javascript:return ConfirmDelete(\"{0}\")", Eval("Navn") != null ? Eval("Navn").ToString() : string.Empty) %>' 
                                    CommandArgument='<%# Eval("Id") %>' OnClick="DeleteButton_Click" ID="DeleteLinkButton" ToolTip="Slett ansatt">
                                    <span class="glyphicon glyphicon-remove"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group btn-edit">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="EditButton_Click" ID="EditLinkButton" ToolTip="Rediger ansatt i detaljvisning">
                                    <span class="glyphicon glyphicon-pencil"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </td>

                        <td>
                            <%--<select data-placeholder="Choose a Country..." class="chosen-select" style="width: 350px;" tabindex="2">
                                <option value=""></option>
                                <option value="United States">United States</option>
                                <option value="United Kingdom">United Kingdom</option>
                                <option value="Afghanistan">Afghanistan</option>
                                <option value="Aland Islands">Aland Islands</option>
                                <option value="Albania">Albania</option>
                                <option value="Algeria">Algeria</option>
                                <option value="American Samoa">American Samoa</option>
                                <option value="Andorra">Andorra</option>
                                <option value="Angola">Angola</option>
                                <option value="Anguilla">Anguilla</option>
                                <option value="Antarctica">Antarctica</option>
                                <option value="Antigua and Barbuda">Antigua and Barbuda</option>
                                <option value="Argentina">Argentina</option>
                                <option value="Armenia">Armenia</option>
                                <option value="Aruba">Aruba</option>
                                <option value="Australia">Australia</option>
                                <option value="Austria">Austria</option>
                                <option value="Azerbaijan">Azerbaijan</option>
                                <option value="Bahamas">Bahamas</option>
                                <option value="Bahrain">Bahrain</option>
                                <option value="Bangladesh">Bangladesh</option>
                                <option value="Barbados">Barbados</option>
                                <option value="Belarus">Belarus</option>
                                <option value="Belgium">Belgium</option>
                                <option value="Belize">Belize</option>
                                <option value="Benin">Benin</option>
                                <option value="Bermuda">Bermuda</option>
                            </select>--%>
                        </td>
                    </tr>
                </ItemTemplate>

            </asp:ListView>

            <%--INFO: no need for paging on this page--%>
            <%--<div class="form-group">
                    <asp:DataPager ID="AnsattItemDataPager" runat="server" PageSize="10" PagedControlID="AnsattListView">
                        <Fields>
                            <asp:NumericPagerField ButtonCount="2" />
                        </Fields>
                    </asp:DataPager>
                </div>--%>
        </div>

    </section>

    <br />

    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Lagre alle" OnClick="SaveAllButton_Click" ID="SaveButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Avbryt"  CausesValidation="false" OnClick="CancelButton_Click" ID="CancelButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Opprett ny ansatt" OnClick="CreateNewButton_Click" ID="CreateNewButtonBottom" />
    
     <%--<link rel="stylesheet" href="../Content/plugins/chosen.min.css">
    <script type="text/javascript" src="../Scripts/plugins/chosen.jquery.min.js"></script>--%>
    <%--<script src="../Scripts/plugins/checkboxDropdown/JavaScriptTest.js"></script>--%>
    <script>

        //$(function () {
        //    $("[id*=JobberIKlasserListBox]").multiselect({
        //        numberDisplayed: 10,
        //        maxHeight: 350,
        //    });
        //});

        //$(function () {
        //    $("[id*=JobberISfosListBox]").multiselect({
        //        numberDisplayed: 10,
        //        maxHeight: 350,
        //    });
        //});

        //$(function () {
        //    $("[id*=AvdelingsLederIAvdelingerListBox]").multiselect({
        //        numberDisplayed: 3,
        //        maxHeight: 350,
        //    });
        //});

        $(function () {

            //Retrieve and use the existing scroll position from the hidden field
            var scrollPosition = $('#<%= ScrollPosition.ClientID %>').val();
            $(window).scrollTop(scrollPosition);

            $(window).scroll(function () {
                var currentScrollPosition = $(window).scrollTop();
                $('#<%= ScrollPosition.ClientID %>').val(currentScrollPosition);
            });
        });

        function ConfirmDelete(name) {
            if (confirm("Er du sikker på at du vil slette ansatt '" + name + "'?")) {
                return true;
            }
            return false;
        }

        //function ScrollToBottom() {
        //    window.scrollTo(0, document.body.scrollHeight);
        //}

            //$(".chosen-select").chosen();

        //$(function () {
        //    $("[id=testselect]").multiselect({
        //        checkAllText: "Velg alle",
        //        uncheckAllText: "Fjern alle",
        //        noneSelectedText: "Ingen valgt",
        //        selectedList: "5"
        //        });

        //});

        //$(document).ready(function () {
        //    $("[id=testselect]").multiselect();
        //});

        //function changeColor(o) {
        //    o.style.backgroundColor = ('red');
        //}

        //function dummyFunction() {
        //    var x = document.getElementById("mySelect").value;
        //    document.getElementById("demo").innerHTML = "You selected: " + x;
        //}

    </script>

</asp:Content>
