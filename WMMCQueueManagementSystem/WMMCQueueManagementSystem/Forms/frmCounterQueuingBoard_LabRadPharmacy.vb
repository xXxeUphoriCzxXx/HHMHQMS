Imports System.Speech.Synthesis

Public Class frmCounterQueuingBoard_LabRadPharmacy
    Private Counters As List(Of Counter)
    Private queuingSpeaker As New System.Speech.Synthesis.SpeechSynthesizer()
    Private callString As New List(Of String), highlightNumber As String = ""
    Private id1 As Long = 0, id2 As Long = 0, id3 As Long = 0, id4 As Long = 0, id5 As Long = 0, id6 As Long = 0, id7 As Long = 0, id8 As Long = 0, id9 As Long = 0, id10 As Long = 0, id11 As Long = 0, id12 As Long = 0, id13 As Long = 0, id14 As Long = 0, id15 As Long = 0, id16 As Long = 0, id17 As Long = 0, id18 As Long = 0, id19 As Long = 0, id20 As Long = 0
    Private counter1 As String = "", counter2 As String = "", counter3 As String = "", counter4 As String = "", counter5 As String = "", counter6 As String = "", counter7 As String = "", counter8 As String = "", counter9 As String = "", counter10 As String = "", counter11 As String = "", counter12 As String = "", counter13 As String = "", counter14 As String = "", counter15 As String = "", counter16 As String = "", counter17 As String = "", counter18 As String = "", counter19 As String = "", counter20 As String = ""
    Private QueueListTimer As Timer = Nothing

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim frm As New frmCounterSelection
        frm.ShowDialog()
        If frm.DialogResult = DialogResult.Yes And Not IsNothing(frm.SelectedCounter) Then
            Dim exist As Boolean = False
            Counters = frm.SelectedCounter
        End If
        Timer1.Interval = 5000
        Timer2.Interval = 150
        callString = New List(Of String)

        Dim callTimer As New Timer With {
            .Interval = 1000
        }
        callTimer.Start()
        AddHandler callTimer.Tick, Sub()
                                       If callString.Count > 0 Then
                                           CallNumber(callString)
                                           callString.Clear()
                                       End If
                                   End Sub
        CLOCK.Start()
        lbwelcome1.Text = Now.ToString("F")
    End Sub

    Private Sub CLOCK_Tick(sender As Object, e As EventArgs) Handles CLOCK.Tick
        lbwelcome1.Text = Now.ToString("F")
    End Sub

    Private Sub Timerwelcome_Tick(sender As Object, e As EventArgs) Handles timerwelcome.Tick
        lbwelcome.Text = MarqueeText(lbwelcome.Text)
        lbwelcome1.Text = MarqueeText(lbwelcome.Text)
    End Sub

    Sub showHelp()
        MessageBox.Show("Note: [F1: Show Help] [F11: toogle fullscreen]", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub FrmCounterQueuingBoard_LabRadPharmacy_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        QueueListTimer.Stop()
        CLOCK.Stop()
        Timer1.Stop()
        Timer2.Stop()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Highlight()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblHighlightServing.Hide()
        lblHighlightServing.SendToBack()
        lblHighlightServing.Visible = False
        Timer1.Stop()
        Timer2.Stop()
    End Sub

    Private Function MarqueeText(ByVal data As String)
        Dim s1 As String = data.Remove(0, 1)
        Dim s2 As String = data(0)
        Return s1 & s2
    End Function

    Private Sub Highlight()
        If lblHighlightServing.BackColor = Color.LimeGreen Then
            lblHighlightServing.BackColor = Color.Aqua
            lblHighlightServing.ForeColor = Color.Black
        ElseIf lblHighlightServing.BackColor = Color.Aqua Then
            lblHighlightServing.BackColor = Color.LimeGreen
            lblHighlightServing.ForeColor = Color.White
        End If
    End Sub

    Private Sub FrmCounterQueuingBoard_LabRadPharmacy_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetPagingConfig()
        CLOCK.Start()
        showHelp()
        If IsNothing(QueueListTimer) Then
            QueueListTimer = New Timer With {
                .Interval = 10000
            }
            AddHandler QueueListTimer.Tick, AddressOf QueueListTimer_Tick
            QueueListTimer.Start()
        Else
            RemoveHandler QueueListTimer.Tick, AddressOf QueueListTimer_Tick
            QueueListTimer = Nothing
            QueueListTimer = New Timer With {
                .Interval = 10000
            }
            AddHandler QueueListTimer.Tick, AddressOf QueueListTimer_Tick
            QueueListTimer.Start()
        End If
    End Sub

    Private Sub FrmCounterQueuingBoard_LabRadPharmacy_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F11 Then
            If Me.WindowState = FormWindowState.Maximized Then
                Me.WindowState = FormWindowState.Normal
                Me.FormBorderStyle = FormBorderStyle.FixedToolWindow
            Else
                Me.WindowState = FormWindowState.Maximized
                Me.FormBorderStyle = FormBorderStyle.None
            End If
        ElseIf e.KeyCode = Keys.F1 Then
            showHelp()
        End If
    End Sub

    Public Sub SetPagingConfig()
        Dim voiceModel As VoiceModel = VoiceSetting
        If voiceModel.VoiceGenderMale Then
            queuingSpeaker.SelectVoiceByHints(VoiceGender.Male)
        Else
            queuingSpeaker.SelectVoiceByHints(VoiceGender.Female)
        End If
        queuingSpeaker.Volume = If(voiceModel.VoiceVolume > 100 Or voiceModel.VoiceVolume < 0, 50, voiceModel.VoiceVolume)
        queuingSpeaker.Rate = voiceModel.VoiceSpeed
    End Sub

    Private Sub CallNumber(str As List(Of String))
        Try
            'queuingSpeaker.SpeakAsyncCancelAll()           
            For Each item In str
                My.Computer.Audio.Play(My.Resources.beep, AudioPlayMode.WaitToComplete)
                queuingSpeaker.SpeakAsync(item)
                lblHighlightServing.Text = highlightNumber.Trim.ToUpper
                lblHighlightServing.BringToFront()
                lblHighlightServing.Visible = True
                lblHighlightServing.Show()
                Timer1.Start()
                Timer2.Start()
            Next
        Catch ex As Exception
            MessageBox.Show("Audio device Error. Please check If your audio Is connected properly", "Audio device Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub QueueListTimer_Tick(sender As Object, e As EventArgs)
        Dim servedCustomerController As New ServedCustomerController
        Dim tmpServingCustomerOfServers As List(Of GetServingCustomerOfServer) = servedCustomerController.GetMultipleDepartmentServingQueue(Me.Counters)
        If Not IsNothing(tmpServingCustomerOfServers) Then
            CheckIfServingChange(tmpServingCustomerOfServers)
            If tmpServingCustomerOfServers.Count > 0 Then
                lblCounter1.Text = If(Not IsNothing(tmpServingCustomerOfServers(0).serverTransaction), tmpServingCustomerOfServers(0).serverTransaction.CounterName, "")
                If Not id1 = tmpServingCustomerOfServers(0).serverTransaction.ServerTransaction_ID Then
                    lbserving1.Text = ""
                End If
                id1 = tmpServingCustomerOfServers(0).serverTransaction.ServerTransaction_ID
                If Not IsNothing(tmpServingCustomerOfServers(0).customerAssigncounter) Then
                    lbserving1.Text = tmpServingCustomerOfServers(0).customerAssigncounter.ProcessedQueueNumber
                    If tmpServingCustomerOfServers(0).customerAssigncounter.Priority > 0 Then
                        lbserving1.ForeColor = Color.IndianRed
                    Else
                        lbserving1.ForeColor = Color.FromArgb(13, 52, 145)
                    End If
                    'CheckIfServingChange(tmpServingCustomerOfServers(1))
                End If
            Else
                lbserving1.ForeColor = Color.DimGray
                lblCounter1.Text = ""
                lbserving1.Text = ""
                id1 = 0

                lbserving2.ForeColor = Color.DimGray
                lblCounter2.Text = ""
                lbserving2.Text = ""
                id2 = 0

                lbserving3.ForeColor = Color.DimGray
                lblCounter3.Text = ""
                lbserving3.Text = ""
                id3 = 0

                lbserving4.ForeColor = Color.DimGray
                lblCounter4.Text = ""
                lbserving4.Text = ""
                id4 = 0

                GoTo SKIP
            End If
            If tmpServingCustomerOfServers.Count > 1 Then
                lblCounter2.Text = If(Not IsNothing(tmpServingCustomerOfServers(1).serverTransaction), tmpServingCustomerOfServers(1).serverTransaction.CounterName, "")
                If Not id2 = tmpServingCustomerOfServers(1).serverTransaction.ServerTransaction_ID Then
                    lbserving2.Text = ""
                End If
                id2 = tmpServingCustomerOfServers(1).serverTransaction.ServerTransaction_ID
                If Not IsNothing(tmpServingCustomerOfServers(1).customerAssigncounter) Then
                    lbserving2.Text = tmpServingCustomerOfServers(1).customerAssigncounter.ProcessedQueueNumber
                    If tmpServingCustomerOfServers(1).customerAssigncounter.Priority > 0 Then
                        lbserving2.ForeColor = Color.IndianRed
                    Else
                        lbserving2.ForeColor = Color.FromArgb(13, 52, 145)
                    End If
                    'CheckIfServingChange(tmpServingCustomerOfServers(1))
                End If
            Else
                lbserving2.ForeColor = Color.DimGray
                lblCounter2.Text = ""
                lbserving2.Text = ""
                id2 = 0

                lbserving3.ForeColor = Color.DimGray
                lblCounter3.Text = ""
                lbserving3.Text = ""
                id3 = 0

                lbserving4.ForeColor = Color.DimGray
                lblCounter4.Text = ""
                lbserving4.Text = ""
                id4 = 0


                GoTo SKIP
            End If
            If tmpServingCustomerOfServers.Count > 2 Then
                lblCounter3.Text = If(Not IsNothing(tmpServingCustomerOfServers(2).serverTransaction), tmpServingCustomerOfServers(2).serverTransaction.CounterName, "")
                If Not id3 = tmpServingCustomerOfServers(2).serverTransaction.ServerTransaction_ID Then
                    lbserving3.Text = ""
                End If
                id3 = tmpServingCustomerOfServers(2).serverTransaction.ServerTransaction_ID
                If Not IsNothing(tmpServingCustomerOfServers(2).customerAssigncounter) Then
                    lbserving3.Text = tmpServingCustomerOfServers(2).customerAssigncounter.ProcessedQueueNumber
                    If tmpServingCustomerOfServers(2).customerAssigncounter.Priority > 0 Then
                        lbserving3.ForeColor = Color.IndianRed
                    Else
                        lbserving3.ForeColor = Color.FromArgb(13, 52, 145)
                    End If
                    'CheckIfServingChange(tmpServingCustomerOfServers(2))
                End If
            Else

                lbserving3.ForeColor = Color.DimGray
                lblCounter3.Text = ""
                lbserving3.Text = ""
                id3 = 0

                lbserving4.ForeColor = Color.DimGray
                lblCounter4.Text = ""
                lbserving4.Text = ""
                id4 = 0


                GoTo SKIP
            End If
            If tmpServingCustomerOfServers.Count > 3 Then
                lblCounter4.Text = If(Not IsNothing(tmpServingCustomerOfServers(3).serverTransaction), tmpServingCustomerOfServers(3).serverTransaction.CounterName, "")
                If Not id4 = tmpServingCustomerOfServers(3).serverTransaction.ServerTransaction_ID Then
                    lbserving4.Text = ""
                End If
                id4 = tmpServingCustomerOfServers(3).serverTransaction.ServerTransaction_ID
                If Not IsNothing(tmpServingCustomerOfServers(3).customerAssigncounter) Then
                    lbserving4.Text = tmpServingCustomerOfServers(3).customerAssigncounter.ProcessedQueueNumber
                    If tmpServingCustomerOfServers(3).customerAssigncounter.Priority > 0 Then
                        lbserving4.ForeColor = Color.IndianRed
                    Else
                        lbserving4.ForeColor = Color.FromArgb(13, 52, 145)
                    End If
                    'CheckIfServingChange(tmpServingCustomerOfServers(3))
                End If
            Else
                lbserving4.ForeColor = Color.DimGray
                lblCounter4.Text = ""
                lbserving4.Text = ""
                id4 = 0


                GoTo SKIP
            End If
        End If
SKIP:
        counter1 = If(lbserving1.Text.ToLower <> "", lbserving1.Text, "")
        counter2 = If(lbserving2.Text.ToLower <> "", lbserving2.Text, "")
        counter3 = If(lbserving3.Text.ToLower <> "", lbserving3.Text, "")
        counter4 = If(lbserving4.Text.ToLower <> "", lbserving4.Text, "")

        'If callString.Count > 0 Then
        '    CallNumber(callString)
        '    callString.Clear()
        'End If
    End Sub

    Private Sub CheckIfServingChange(tmpServingCustomerOfServers As List(Of GetServingCustomerOfServer))
        For Each CustomerOfServers In tmpServingCustomerOfServers
            If IsNothing(CustomerOfServers.customerAssigncounter) Then
                Continue For
            End If
            If id1 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter1.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id2 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter2.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id3 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter3.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id4 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter4.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id5 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter5.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id6 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter6.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id7 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter7.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id8 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter8.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id9 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter9.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id10 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter10.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id11 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter11.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id12 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter12.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id13 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter13.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id14 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter14.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id15 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter15.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id16 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter16.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id17 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter17.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            ElseIf id18 = CustomerOfServers.serverTransaction.ServerTransaction_ID Then
                If counter18.ToLower <> CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.ToLower Then
                    Dim item = CustomerOfServers.customerAssigncounter.ProcessedQueueNumber.Replace("-", "")
                    Dim queueNumberlst As Char() = item.ToCharArray()
                    Dim itemNew As String = ""
                    For Each ltr In queueNumberlst
                        itemNew &= ltr & " "
                    Next
                    callString.Add("Now Serving, " & itemNew & ", please proceed to " & CustomerOfServers.serverTransaction.CounterName & ".")
                    ''callString = callString & tmpServingCustomerOfServers.customerAssigncounter.ProcessedQueueNumber & ", "
                    highlightNumber = CustomerOfServers.serverTransaction.CounterName + vbCrLf + CustomerOfServers.customerAssigncounter.ProcessedQueueNumber
                End If
            End If

        Next
    End Sub
End Class