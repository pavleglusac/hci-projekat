using HCIProjekat.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCIProjekat.views.manager.pages
{
    /// <summary>
    /// Interaction logic for TrainAddition.xaml
    /// </summary>
    public partial class SeatCreator : Page
    {


        List<Rectangle> emptySeats = new List<Rectangle>();
        List<Rectangle> seats = new List<Rectangle>();

        List<Row> rows = new List<Row>();

        Dictionary<Rectangle, Row> seatParent = new Dictionary<Rectangle, Row>();

        Dictionary<Border, Row> borderParent = new Dictionary<Border, Row>();

        Rectangle seat, newSeat, trash;

        Border newRow;

        public static RoutedCommand RowAddCommand = new RoutedCommand();
        public static RoutedCommand RowRemoveCommand = new RoutedCommand();
        public static RoutedCommand SeatAddCommand = new RoutedCommand();
        public static RoutedCommand SeatRemoveCommand = new RoutedCommand();

        public static RoutedCommand RowUpCommand = new RoutedCommand();
        public static RoutedCommand RowDownCommand = new RoutedCommand();
        public static RoutedCommand RowLeftCommand = new RoutedCommand();
        public static RoutedCommand RowRightCommand = new RoutedCommand();

        public static RoutedCommand RowSwapUpCommand = new RoutedCommand();
        public static RoutedCommand RowSwapDownCommand = new RoutedCommand();

        public static RoutedCommand UndoCommand = new RoutedCommand();
        public static RoutedCommand RedoCommand = new RoutedCommand();

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand RemoveAllCommand = new RoutedCommand();

        object SelectedElement;

        SolidColorBrush highlightBrush = new SolidColorBrush(Colors.Gray);
        SolidColorBrush regularBrush = new SolidColorBrush(Colors.Black);


        Dictionary<Row, Rectangle> rowEmptySeat = new Dictionary<Row, Rectangle>();

        int SelectedIndex = -1;

        Border row, trow, brow;

        TrainHistory history = new TrainHistory();

        Train train;

        void AddTools()
        {
            row = RowBuilder.buildSingleRow();
            toolGrid.Children.Add(row);
            row.HorizontalAlignment = HorizontalAlignment.Center;
            row.MouseLeftButtonDown += row_MouseLeftButtonDown;
            row.MouseMove += row_MouseMove;
            row.MouseLeftButtonUp += row_MouseLeftButtonUp;
            Grid.SetRow(row, 0);

            trow = RowBuilder.buildTopRow();
            toolGrid.Children.Add(trow);
            trow.HorizontalAlignment = HorizontalAlignment.Center;
            trow.MouseLeftButtonDown += rowTop_MouseLeftButtonDown;
            trow.MouseMove += rowTop_MouseMove;
            trow.MouseLeftButtonUp += rowTop_MouseLeftButtonUp;
            Grid.SetRow(trow, 2);


            brow = RowBuilder.buildBottomRow();
            brow.HorizontalAlignment = HorizontalAlignment.Center;
            brow.MouseLeftButtonDown += rowBottom_MouseLeftButtonDown;
            brow.MouseMove += rowBottom_MouseMove;
            brow.MouseLeftButtonUp += rowBottom_MouseLeftButtonUp;
            toolGrid.Children.Add(brow);
            Grid.SetRow(brow, 4);

            seat = SeatBuilder.buildSeat();
            toolGrid.Children.Add(seat);
            Grid.SetRow(seat, 6);
            Grid.SetZIndex(seat, 100);
            seat.MouseLeftButtonDown += root_MouseLeftButtonDown;
            seat.MouseMove += root_MouseMove;
            seat.MouseLeftButtonUp += root_MouseLeftButtonUp;


            trash = SeatBuilder.buildTrash();
            toolGrid.Children.Add(trash);
            Grid.SetRow(trash, 8);
        }

        void AddShortcuts()
        {
            RowAddCommand.InputGestures.Add(new KeyGesture(Key.Down, ModifierKeys.Control));
            RowRemoveCommand.InputGestures.Add(new KeyGesture(Key.Up, ModifierKeys.Control));
            SeatAddCommand.InputGestures.Add(new KeyGesture(Key.Right, ModifierKeys.Control));
            SeatRemoveCommand.InputGestures.Add(new KeyGesture(Key.Left, ModifierKeys.Control));
            RowUpCommand.InputGestures.Add(new KeyGesture(Key.Up));
            RowDownCommand.InputGestures.Add(new KeyGesture(Key.Down));
            RowLeftCommand.InputGestures.Add(new KeyGesture(Key.Left));
            RowRightCommand.InputGestures.Add(new KeyGesture(Key.Right));
            RowSwapUpCommand.InputGestures.Add(new KeyGesture(Key.Up, ModifierKeys.Alt));
            RowSwapDownCommand.InputGestures.Add(new KeyGesture(Key.Down, ModifierKeys.Alt));

            UndoCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            RedoCommand.InputGestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control));

            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            RemoveAllCommand.InputGestures.Add(new KeyGesture(Key.Delete, ModifierKeys.Control));
        }

        public SeatCreator()
        {
            InitializeComponent();
            this.Focus();

            AddTools();
            AddShortcuts();

            Row row0 = new Row();
            row0.RowBorder = RowBuilder.buildSingleRow();
            row0.RowBorder.MouseMove += row_MouseMove;
            row0.RowBorder.MouseLeftButtonUp += row_MouseLeftButtonUp;
            row0.RowUI = (StackPanel)row0.RowBorder.Child;
            row0.RowType = RowEnum.ALL;
            row0.LeftRow = true;
            borderParent.Add(row0.RowBorder, row0);
            rows.Add(row0);

            Row row1 = new Row();
            row1.RowBorder = RowBuilder.buildTopRow();
            row1.RowBorder.MouseMove += row_MouseMove;
            row1.RowBorder.MouseLeftButtonUp += row_MouseLeftButtonUp;
            row1.RowUI = (StackPanel)row1.RowBorder.Child;
            row1.RowType = RowEnum.TOP;
            row1.LeftRow = true;
            borderParent.Add(row1.RowBorder, row1);
            rows.Add(row1);

            SetNumberLabels();


            Row row2 = new Row();
            row2.RowBorder = RowBuilder.buildBottomRow();
            row2.RowBorder.MouseMove += row_MouseMove;
            row2.RowBorder.MouseLeftButtonUp += row_MouseLeftButtonUp;
            row2.RowUI = (StackPanel)row2.RowBorder.Child;
            row2.RowType = RowEnum.BOTTOM;
            row2.LeftRow = true;
            borderParent.Add(row2.RowBorder, row2);
            rows.Add(row2);

            Row row3 = new Row();
            row3.RowBorder = RowBuilder.buildBottomRow();
            row3.RowBorder.MouseMove += row_MouseMove;
            row3.RowBorder.MouseLeftButtonUp += row_MouseLeftButtonUp;
            row3.RowUI = (StackPanel)row3.RowBorder.Child;
            row3.RowType = RowEnum.BOTTOM;
            row3.LeftRow = false;
            borderParent.Add(row3.RowBorder, row3);
            rows.Add(row3);

            int j = 0;
            foreach (var tempRow in rows)
            {
                for (int i = 0; i < 2; i++)
                {
                    var item = SeatBuilder.buildSeat();
                    tempRow.Seats.Add(item);
                    item.Margin = new Thickness(5, 5, 5, 5);
                    tempRow.RowUI.Children.Add(item);
                    seatParent.Add(item, tempRow);
                }
                Rectangle emptySeat = SeatBuilder.buildEmptySeat();
                emptySeat.Visibility = Visibility.Collapsed;
                emptySeat.Margin = new Thickness(5, 5, 5, 5);
                emptySeats.Add(emptySeat);
                tempRow.RowUI.Children.Add(emptySeat);
                tempRow.RowBorder.MouseDown += SelectRow;
                rowEmptySeat[tempRow] = emptySeat;
                seatParent.Add(emptySeat, tempRow);
                tempRow.RowBorder.Margin = new Thickness(0, 5, 0, 5);
                if (tempRow.LeftRow)
                    leftRowStack.Children.Add(tempRow.RowBorder);
                else
                    rightRowStack.Children.Add(tempRow.RowBorder);
                j++;
            }
            SetNumberLabels();
            AdjustRowWidth();
        }

        public SeatCreator(Train train)
        {
            InitializeComponent();
            this.train = train;
            this.Focus();
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                HelpProvider.SetHelpKey((DependencyObject)focusedControl, "seats");
            }
            AddTools();
            AddShortcuts();
            ConvertTrainToUI(train);
            UndoButton.IsEnabled = history.CanUndo();
            RedoButton.IsEnabled = history.CanRedo();
            history.AddTrain(train);
        }

        void ConvertTrainToUI(Train train)
        {
            TrainNameInput.Text = train.Name;
            foreach (model.Row row in train.LeftRows)
            {
                AddRow(row, true);
            }
            foreach (model.Row row in train.RightRows)
            {
                AddRow(row, false);
            }
            SetNumberLabels();
            AdjustRowWidth();
        }

        void AddRow(model.Row row, Boolean left)
        {
            Row newRow = new Row();
            newRow.RowBorder = RowBuilder.buildByType(row.RowType);
            borderParent[newRow.RowBorder] = newRow;
            newRow.RowUI = (StackPanel)newRow.RowBorder.Child;
            newRow.RowType = row.RowType;
            newRow.LeftRow = left;
            newRow.Seats.AddRange(row.Seats.Select(
                x => {
                    var seat = SeatBuilder.buildSeat();
                    newRow.RowUI.Children.Add(seat);
                    seat.MouseLeftButtonDown += root_MouseLeftButtonDown;
                    seat.MouseLeftButtonUp += root_MouseLeftButtonUp;
                    seat.MouseMove += root_MouseMove;
                    seatParent.Add(seat, newRow);
                    return seat;
                }
            ).ToList());
            
            Rectangle emptySeat = SeatBuilder.buildEmptySeat();
            emptySeat.Visibility = Visibility.Collapsed;
            emptySeats.Add(emptySeat);
            newRow.RowUI.Children.Add(emptySeat);
            rowEmptySeat[newRow] = emptySeat;
            seatParent.Add(emptySeat, newRow);
            
            newRow.RowBorder.MouseDown += SelectRow;
            newRow.RowBorder.Margin = new Thickness(0, 5, 0, 5);
            if (left)
                leftRowStack.Children.Add(newRow.RowBorder);
            else
                rightRowStack.Children.Add(newRow.RowBorder);
            rows.Add(newRow);
        }

        Train ConvertUIToTrain()
        {
            Train newTrain = new Train();
            newTrain.PricePerMinute = train.PricePerMinute;
            newTrain.CreationDate = train.CreationDate;
            newTrain.Name = TrainNameInput.Text;
            foreach (Row row in rows)
            {
                model.Row modelRow = new model.Row();
                modelRow.RowType = row.RowType;
                modelRow.Seats.AddRange(
                    row.Seats.Select(x =>
                    {
                        model.Seat modelSeat = new model.Seat();
                        return modelSeat;
                    }).ToList()
                );
                if (row.LeftRow)
                    newTrain.LeftRows.Add(modelRow);
                else
                    newTrain.RightRows.Add(modelRow);
            }
            return newTrain;
        }

        void HistoryAction()
        {
            Train train = ConvertUIToTrain();
            train.SetSeatLabels();
            history.AddTrain(train);
            UndoButton.IsEnabled = history.CanUndo();
            RedoButton.IsEnabled = history.CanRedo();
            System.Diagnostics.Debug.WriteLine($"{train.ToString()}");
        }

        private void SetNumberLabels()
        {
            int maxLeft = 0;
            int maxRight = 0;
            int maxRowsLeft = 0;
            int maxRowsRight = 0;

            leftNumbersStack.Children.Clear();
            rightNumbersStack.Children.Clear();
            rowNumbersStack.Children.Clear();

            foreach (var row in rows)
            {
                if (row.LeftRow)
                {
                    maxLeft = Math.Max(maxLeft, row.Seats.Count);
                    maxRowsLeft++;
                }
                else
                {
                    maxRight = Math.Max(maxRight, row.Seats.Count);
                    maxRowsRight++;
                }

            }

            int totalCount = 0;

            for (int i = 0; i < maxLeft; i++)
            {
                Label label1 = new Label();
                char a = 'A';
                label1.Content = (char)(a + totalCount++);
                label1.Width = 60;
                if (i == 0) label1.Margin = new Thickness(38, 0, 0, 0);
                label1.HorizontalAlignment = HorizontalAlignment.Center;
                leftNumbersStack.Children.Add(label1);
            }

            for (int i = 0; i < maxRight; i++)
            {
                Label label1 = new Label();
                char a = 'A';
                label1.Content = (char)(a + totalCount++);
                label1.Width = 60;
                if (i == 0) label1.Margin = new Thickness(38, 0, 0, 0);
                label1.HorizontalAlignment = HorizontalAlignment.Center;
                rightNumbersStack.Children.Add(label1);
            }


            for (int i = 0; i < Math.Max(maxRowsLeft, maxRowsRight); i++)
            {
                Label label1 = new Label();
                label1.Content = (i + 1).ToString();
                label1.Height = 70;
                if (i == 0) label1.Margin = new Thickness(0, 30, 0, 0);
                label1.HorizontalAlignment = HorizontalAlignment.Center;
                rowNumbersStack.Children.Add(label1);
            }
        }

        private void ClearRows()
        {
            leftRowStack.Children.Clear();
            rightRowStack.Children.Clear();
        }

        private void RedrawRows()
        {
            foreach (var tempRow in rows)
            {
                tempRow.RowBorder.Margin = new Thickness(0, 5, 0, 5);
                if (tempRow.LeftRow)
                    leftRowStack.Children.Add(tempRow.RowBorder);
                else
                    rightRowStack.Children.Add(tempRow.RowBorder);
            }
        }

        private void ClearAll()
        {
            ClearRows();
            emptySeats.Clear();
            rowEmptySeat.Clear();
            borderParent.Clear();
            rows.Clear();
            seats.Clear();
            SelectedElement = null;
            SelectedIndex = -1;
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if(history.CanUndo())
            {
                Train train = history.Undo();
                ClearAll();
                ConvertTrainToUI(train);
            }
            UndoButton.IsEnabled = history.CanUndo();
            RedoButton.IsEnabled = history.CanRedo();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if(history.CanRedo())
            {
                Train train = history.Redo();
                ClearAll();
                ConvertTrainToUI(train);
            }
            UndoButton.IsEnabled = history.CanUndo();
            RedoButton.IsEnabled = history.CanRedo();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string message = "Da li ste sigurni da želite da sačuvate voz?";
            string caption = "Potvrda";

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                HistoryAction();
                Train ct = history.CurrentTrain();
                Database.UpdateTrain(history.History[0], ct);
            }
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            string message = "Da li ste sigurni da želite da uklonite sva sedišta i sve redove?";
            string caption = "Potvrda";
            
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                HistoryAction();
                ClearRows();
                ClearAll();
                HistoryAction();
            }
        }

        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Undo_Click(sender, e);
        }

        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Redo_Click(sender, e);
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Click(sender, e);
        }

        private void RemoveAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteAll_Click(sender, e);
        }

        private void AdjustRowWidth(bool empty = false)
        {
            
            foreach (var row in rows)
            {
                if (!empty || row.Seats.Count >= 4)
                {
                    row.RowUI.Width = Math.Max(row.Seats.Count * 60, 20);
                    row.RowBorder.Width = Math.Max(row.Seats.Count * 60 + 5, 20);
                }
                else
                {
                    row.RowUI.Width = Math.Max((row.Seats.Count + 1) * 60, 20);
                    row.RowBorder.Width = Math.Max((row.Seats.Count + 1) * 60 + 5, 20);
                }
            }
        }

        // COMMANDS

        private void RowAddCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedElement == null)
            {
                return;
            }
            if (SelectedElement.GetType() == typeof(Border))
            {
                Border border = (Border)SelectedElement;
                var parent = borderParent[border];
                if(parent.LeftRow)
                {
                    if (leftRowStack.Children.Count >= 7) return;
                }
                else
                {
                    if (rightRowStack.Children.Count >= 7) return;
                }
                Row clone = parent.DeepCopy();
                SetRowEvents(clone, clone.RowType);
                Rectangle emptySeat = SeatBuilder.buildEmptySeat();
                emptySeat.Visibility = Visibility.Collapsed;
                emptySeat.Margin = new Thickness(5, 5, 5, 5);
                emptySeats.Add(emptySeat);
                clone.RowUI.Children.Add(emptySeat);
                rowEmptySeat[clone] = emptySeat;
                seatParent.Add(emptySeat, clone);
                int ind = rows.IndexOf(parent);
                rows.Insert(ind, clone);
                SelectedIndex = ind + 1;
                scrollViewer.ScrollToVerticalOffset(ind * 60);
                borderParent[clone.RowBorder] = clone;
                if (clone.LeftRow)
                {
                    leftRowStack.Children.Add(clone.RowBorder);
                }
                else
                {
                    rightRowStack.Children.Add(clone.RowBorder);
                }
                SetNumberLabels();
                ClearRows();
                RedrawRows();
                HistoryAction();
            }
        }

        private void RowRemoveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedElement == null)
            {
                return;
            }
            if (SelectedElement.GetType() == typeof(Border))
            {
            }
        }

        private void SeatAddCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedElement == null)
            {
                return;
            }
            if (SelectedElement.GetType() == typeof(Border))
            {
                Border border = (Border)SelectedElement;
                var parent = borderParent[border];
                if (parent.Seats.Count >= 4) return;
                var seat = SeatBuilder.buildSeat();
                seat.MouseLeftButtonDown += root_MouseLeftButtonDown;
                seat.MouseMove += root_MouseMove;
                seat.MouseLeftButtonUp += root_MouseLeftButtonUp;
                seat.Margin = new Thickness(5, 5, 5, 5);
                var emptySeat = rowEmptySeat[parent];
                parent.RowUI.Children.Remove(emptySeat);
                parent.Seats.Add(seat);
                parent.RowUI.Children.Add(seat);
                parent.RowUI.Children.Add(emptySeat);
                SetNumberLabels();
                AdjustRowWidth();
                ClearRows();
                RedrawRows();
                HistoryAction();
            }
        }

        private void SeatRemoveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedElement == null)
            {
                return;
            }
            if (SelectedElement.GetType() == typeof(Border))
            {

            }
        }

        private void RowUpCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedElement == null)
            {
                return;
            }
            if (SelectedElement.GetType() == typeof(Border))
            {
                Border border = (Border)SelectedElement;
                var parent = borderParent[border];
                Row clone = parent.DeepCopy();
                int ind = rows.IndexOf(parent);
                if (ind == 0) return;
                var brd = rows[ind - 1].RowBorder;
                SelectedIndex = ind - 1;
                scrollViewer.ScrollToVerticalOffset(indexToRowIndex(rows[ind - 1].LeftRow, ind - 1) * 60);
                if (SelectedElement != null)
                {
                    ((Border)SelectedElement).BorderBrush = regularBrush;
                }
                SelectedElement = brd;
                ((Border)SelectedElement).BorderBrush = highlightBrush;
                HistoryAction();
            }
        }

        private void RowDownCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedElement == null)
            {
                if (rows.Count > 0)
                {
                    SelectedElement = rows[0].RowBorder;
                    ((Border)SelectedElement).BorderBrush = highlightBrush;
                    SelectedIndex = 0;
                }
                return;
            }
            if (SelectedElement.GetType() == typeof(Border))
            {
                Border border = (Border)SelectedElement;
                var parent = borderParent[border];
                Row clone = parent.DeepCopy();
                int ind = rows.IndexOf(parent);
                if (ind >= rows.Count - 1) return;
                var brd = rows[ind + 1].RowBorder;
                scrollViewer.ScrollToVerticalOffset(indexToRowIndex(rows[ind + 1].LeftRow, ind + 1) * 60);
                SelectedIndex = ind + 1;
                if (SelectedElement != null)
                {
                    ((Border)SelectedElement).BorderBrush = regularBrush;
                }
                SelectedElement = brd;
                ((Border)SelectedElement).BorderBrush = highlightBrush;
                HistoryAction();
            }
        }

        private void RowRightCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine(SelectedIndex);
            if (SelectedElement == null)
            {
                if (rows.Count > 0)
                {
                    SelectedElement = rows[rowIndexToIndex(false, 0)].RowBorder;
                    ((Border)SelectedElement).BorderBrush = highlightBrush;
                    SelectedIndex = 0;
                }
            }
            else
            {
                if (!rows[SelectedIndex].LeftRow) return;
                var maxR = rowsInStack(false);
                var selectedRowIndex = indexToRowIndex(true, SelectedIndex);
                var ind = selectedRowIndex > maxR - 1 ? maxR - 1 : selectedRowIndex;
                ((Border)SelectedElement).BorderBrush = regularBrush;
                SelectedElement = rows[rowIndexToIndex(false, ind)].RowBorder;
                ((Border)SelectedElement).BorderBrush = highlightBrush;
                SelectedIndex = rowIndexToIndex(false, ind);
                scrollViewer.ScrollToVerticalOffset(ind * 60);
                HistoryAction();
            }
        }

        private void RowLeftCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine(SelectedIndex);
            if (SelectedElement == null)
            {
                if (rows.Count > 0)
                {
                    SelectedElement = rows[rowIndexToIndex(true, 0)].RowBorder;
                    ((Border)SelectedElement).BorderBrush = highlightBrush;
                    SelectedIndex = 0;
                }
            }
            else
            {
                if (rows[SelectedIndex].LeftRow) return;
                var maxR = rowsInStack(true);
                var selectedRowIndex = indexToRowIndex(false, SelectedIndex);
                var ind = selectedRowIndex > maxR - 1 ? maxR - 1 : selectedRowIndex;
                ((Border)SelectedElement).BorderBrush = regularBrush;
                SelectedElement = rows[rowIndexToIndex(true, ind)].RowBorder;
                ((Border)SelectedElement).BorderBrush = highlightBrush;
                SelectedIndex = rowIndexToIndex(true, ind);
                scrollViewer.ScrollToVerticalOffset(ind * 60);
                HistoryAction();
            }
        }

        private void RowSwapUpCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedElement == null)
            {
                return;
            }
            if (SelectedElement.GetType() == typeof(Border))
            {
                var left = rows[SelectedIndex].LeftRow;
                var ind = indexToRowIndex(left, SelectedIndex);
                if (ind == 0) return;

                List<Row> side = GetRowSide(left);
                if (left)
                {
                    var child = leftRowStack.Children[ind];
                    leftRowStack.Children.RemoveAt(ind);
                    leftRowStack.Children.Insert(ind - 1, child);
                }
                else
                {
                    var child = rightRowStack.Children[ind];
                    rightRowStack.Children.RemoveAt(ind);
                    rightRowStack.Children.Insert(ind - 1, child);
                }
                Swap<Row>(rows, rowIndexToIndex(left, ind), rowIndexToIndex(left, ind - 1));
                SelectedIndex = rowIndexToIndex(left, ind - 1);
                scrollViewer.ScrollToVerticalOffset((ind - 1) * 60);
                HistoryAction();
            }
        }

        private void RowSwapDownCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedElement == null)
            {
                return;
            }
            if (SelectedElement.GetType() == typeof(Border))
            {
                var left = rows[SelectedIndex].LeftRow;
                var ind = indexToRowIndex(left, SelectedIndex);
                var total = rowsInStack(left);
                if (ind >= total - 1) return;
                if (left)
                {
                    var child = leftRowStack.Children[ind];
                    leftRowStack.Children.RemoveAt(ind);
                    leftRowStack.Children.Insert(ind + 1, child);
                }
                else
                {
                    var child = rightRowStack.Children[ind];
                    rightRowStack.Children.RemoveAt(ind);
                    rightRowStack.Children.Insert(ind + 1, child);
                }
                Swap<Row>(rows, rowIndexToIndex(left, ind), rowIndexToIndex(left, ind + 1));
                SelectedIndex = rowIndexToIndex(left, ind + 1);
                scrollViewer.ScrollToVerticalOffset((ind + 1) * 60);
                HistoryAction();
            }
        }

        private int indexToRowIndex(bool left, int ind)
        {
            int total = 0;
            for (int i = 0; i < ind; i++)
            {
                if (rows[i].LeftRow == left) total++;
            }
            return total;
        }

        private int rowIndexToIndex(bool left, int ind)
        {
            int total = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].LeftRow == left)
                {
                    if (total == ind) return i;
                    total++;
                }
            }
            return 0;
        }

        private int rowsInStack(bool left)
        {
            int total = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].LeftRow == left) total++;
            }
            return total;
        }

        public IList<T> Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        List<Row> GetRowSide(bool left)
        {
            List<Row> sideRow = new List<Row>();
            foreach (Row row in rows)
            {
                if (row.LeftRow == left)
                    sideRow.Add(row);
            }
            return sideRow;
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }


        private void SelectRow(object sender, MouseEventArgs e)
        {
            if (SelectedElement != null)
            {
                ((Border)SelectedElement).BorderBrush = regularBrush;
            }
            if (sender == null) return;
            SelectedElement = sender;
            ((Border)SelectedElement).BorderBrush = highlightBrush;
            SelectedIndex = SelectIndexFromSelectElement();
        }

        private int SelectIndexFromSelectElement()
        {
            int index = 0;
            foreach (Row row in rows)
            {
                if (SelectedElement == row.RowBorder)
                {
                    return index;
                }
                index++;
            }
            return index;
        }

        // MOVE SEAT

        Point anchorPoint;
        Point currentPoint;
        bool isInDrag = false;
        bool isRowInDrag = false;
        bool isBrowInDrag = false;
        bool isTrowInDrag = false;
        bool isSeatInDrag = false;
        private void root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Rectangle;
            anchorPoint = e.GetPosition(null);
            element.CaptureMouse();
            if (!transforms.ContainsKey(element))
            {
                transforms.Add(element, new TranslateTransform());
            }
            if (seatParent.ContainsKey(element))
            {

                Point tl = GetPointOfControl(element);
                seatParent[element].RowUI.Children.Remove(element);
                seatParent[element].Seats.Remove(element);
                toolGrid.Children.Add(element);
                Grid.SetRow(element, 6);
                Point p = GetPointOfControl(seat);
                transforms[element].X += tl.X - p.X;
                transforms[element].Y += tl.Y - p.Y;
                element.RenderTransform = transforms[element];
            }
            isSeatInDrag = true;
            e.Handled = true;
        }

        Dictionary<Rectangle, TranslateTransform> transforms = new Dictionary<Rectangle, TranslateTransform>();
        Dictionary<Border, TranslateTransform> transformsRow = new Dictionary<Border, TranslateTransform>();


        private void root_MouseMove(object sender, MouseEventArgs e)
        {
            if (isSeatInDrag)
            {

                if (newSeat == null)
                {
                    newSeat = SeatBuilder.buildSeat();
                    toolGrid.Children.Add(newSeat);
                    Grid.SetRow(newSeat, 6);
                }

                HandleSeatDragOver(sender, e);
            }
        }

        private void root_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isSeatInDrag)
            {
                var element = sender as Rectangle;
                element.ReleaseMouseCapture();
                isSeatInDrag = false;
                e.Handled = true;
                if (!inDelete)
                {
                    if (emptySnapped != null)
                    {
                        if (seatParent.ContainsKey(element))
                        {
                            toolGrid.Children.Remove(element);
                            seatParent[emptySnapped].Seats.Add(element);
                            seatParent[emptySnapped].RowUI.Children.Remove(emptySnapped);
                            seatParent[emptySnapped].RowUI.Children.Add(element);
                            seatParent[emptySnapped].RowUI.Children.Add(emptySnapped);
                            seatParent[element] = seatParent[emptySnapped];
                            SetNumberLabels();
                            emptySnapped = null;
                        }
                        else
                        {
                            if(seatParent[emptySnapped].Seats.Count >= 4) return;
                            Rectangle newSeat = CopyRectangle(seat);
                            seatParent[emptySnapped].RowUI.Children.Remove(emptySnapped);
                            seatParent[emptySnapped].Seats.Add(newSeat);
                            seatParent[emptySnapped].RowUI.Children.Add(newSeat);
                            seatParent[emptySnapped].RowUI.Children.Add(emptySnapped);
                            seatParent[newSeat] = seatParent[emptySnapped];
                            newSeat.MouseLeftButtonDown += root_MouseLeftButtonDown;
                            newSeat.MouseMove += root_MouseMove;
                            newSeat.MouseLeftButtonUp += root_MouseLeftButtonUp;
                            newSeat.Margin = new Thickness(5, 5, 5, 5);
                            SetNumberLabels();
                            emptySnapped = null;
                        }
                        HistoryAction();
                    }
                    else
                    {
                        if (seatParent.ContainsKey(element))
                        {
                            toolGrid.Children.Remove(element);
                            var emptySnapped = rowEmptySeat[seatParent[element]];
                            seatParent[emptySnapped].RowUI.Children.Remove(emptySnapped);
                            seatParent[element].RowUI.Children.Add(element);
                            seatParent[element].RowUI.Children.Add(emptySnapped);
                            seatParent[element].Seats.Add(element);
                            HistoryAction();
                        }
                    }
                    toolGrid.Children.Remove(newSeat);
                    transforms[element].X = 0;
                    transforms[element].Y = 0;
                    newSeat = null;
                    foreach (Rectangle emptySeat in emptySeats)
                    {
                        emptySeat.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    toolGrid.Children.Remove(element);
                    trash.Opacity = 1;
                    HistoryAction();
                }
                AdjustRowWidth();
            }
            HideEmptySeats();
            SetNumberLabels();
        }

        private void HideEmptySeats()
        {
            foreach(var emptySeat in emptySeats)
            {
                emptySeat.Visibility = Visibility.Collapsed;
            }
        }

        private Rectangle CopyRectangle(Rectangle r)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Width = r.Width;
            rectangle.Height = r.Height;
            rectangle.Fill = r.Fill;
            rectangle.Margin = r.Margin;
            rectangle.Stroke = r.Stroke;
            rectangle.Visibility = r.Visibility;
            return rectangle;
        }

        bool snapped = false;
        bool inDelete = false;
        Rectangle emptySnapped;
        private void HandleSeatDragOver(object sender, MouseEventArgs e)
        {
            var element = sender as Rectangle;
            currentPoint = e.GetPosition(null);
            bool was = false;
            bool wasDeleted = false;
            foreach (Rectangle emptySeat in emptySeats)
            {
                Row parent = seatParent[emptySeat];
                if (parent.Seats.Count >= 4) continue;
                Point tl = GetPointOfControl(emptySeat);
                emptySeat.Visibility = Visibility.Visible;
                Rect rect = new Rect(new Point(tl.X - 5, tl.Y - 5), new Point(tl.X + 55, tl.Y + 55));
                if (rect.Contains(currentPoint))
                {
                    Point p = GetPointOfControl(element);
                    if (!snapped)
                    {
                        transforms[element].X += tl.X - p.X;
                        transforms[element].Y += tl.Y - p.Y;
                        element.RenderTransform = transforms[element];
                        anchorPoint = new Point(tl.X + SeatBuilder.W / 2, tl.Y + SeatBuilder.H / 2);
                        snapped = true;
                        emptySnapped = emptySeat;
                    }
                    was = true;
                }
            }
            {
                Point tl = GetPointOfControl(trash);
                Rect rect = new Rect(new Point(tl.X - 50, tl.Y - 50), new Point(tl.X + 80, tl.Y + 80));
                if (rect.Contains(currentPoint))
                {

                    Point p = GetPointOfControl(element);
                    if (!inDelete)
                    {
                        trash.Opacity = 0.5;
                        inDelete = true;
                    }
                    wasDeleted = true;
                }
            }
            if (!snapped)
            {
                if (!transforms.ContainsKey(element)) return;
                transforms[element].X += currentPoint.X - anchorPoint.X;
                transforms[element].Y += currentPoint.Y - anchorPoint.Y;
                element.RenderTransform = transforms[element];
                anchorPoint = currentPoint;
            }
            else
            {
                if (!was)
                {
                    if (!transforms.ContainsKey(element)) return;
                    snapped = false;
                    emptySnapped = null;
                    transforms[element].X += currentPoint.X - anchorPoint.X;
                    transforms[element].Y += currentPoint.Y - anchorPoint.Y;
                    element.RenderTransform = transforms[element];
                    anchorPoint = currentPoint;
                }
            }
            if (inDelete && !wasDeleted)
            {
                inDelete = false;
                trash.Opacity = 1;
            }
            AdjustRowWidth(true);
        }

        private Point GetPointOfControl(UIElement rootVisual)
        {
            try
            {
                Point relativePoint = rootVisual.TransformToAncestor(Window.GetWindow(this)).Transform(new Point(0, 0));
                return relativePoint;
            }
            catch (Exception ex)
            {
                return new Point(0, 0);
            }
        }

        // MOVE ROW

        Border emptyRowSnapped;
        private void row_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Border;
            if (!ButtonDownRowLogic(sender, e)) return;
            anchorPoint = e.GetPosition(null);
            element.CaptureMouse();
            isRowInDrag = true;
            e.Handled = true;
            RemoveEmptyRows();
            AddEmptyRows(RowBuilder.buildSingleRow);
        }

        private void rowTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Border;
            if (!ButtonDownRowLogic(sender, e)) return;
            element.CaptureMouse();
            isTrowInDrag = true;
            e.Handled = true;
            RemoveEmptyRows();
            AddEmptyRows(RowBuilder.buildTopRow);
        }

        private void rowBottom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Border;
            if (!ButtonDownRowLogic(sender, e)) return;
            element.CaptureMouse();
            isBrowInDrag = true;
            e.Handled = true;
            RemoveEmptyRows();
            AddEmptyRows(RowBuilder.buildBottomRow);
        }

        private bool ButtonDownRowLogic(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Border;
            SelectRow(sender, e);
            if (!transformsRow.ContainsKey(element))
            {
                transformsRow.Add(element, new TranslateTransform());
            }
            if (borderParent.ContainsKey(element))
            {
                if (borderParent[element].Seats.Count() != 0) return false;
                Point tl = GetPointOfControl(element);
                rightRowStack.Children.Remove(element);
                leftRowStack.Children.Remove(element);
                toolGrid.Children.Add(element);
                Grid.SetRow(element, 0);
                Point p = GetPointOfControl(row);
                transformsRow[element].X = tl.X - p.X;
                transformsRow[element].Y = tl.Y - p.Y;
                element.RenderTransform = transformsRow[element];
                element.Width = RowBuilder.W;
                element.Height = RowBuilder.H;
                (element.Child as StackPanel).Width = RowBuilder.W;
                (element.Child as StackPanel).Height = RowBuilder.H;
                rows.Remove(borderParent[element]);
            }
            anchorPoint = e.GetPosition(null);
            return true;
        }

        private void row_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRowInDrag)
            {
                if (newRow == null)
                {
                    newRow = RowBuilder.buildSingleRow();
                    newRow.HorizontalAlignment = HorizontalAlignment.Center;
                    toolGrid.Children.Add(newRow);
                    newRow.MouseMove += row_MouseMove;
                    newRow.MouseLeftButtonDown += row_MouseLeftButtonDown;
                    newRow.MouseLeftButtonUp += row_MouseLeftButtonUp;
                    Grid.SetRow(newRow, 0);
                }
                HandleRowDragOver(sender, e, row);
            }
        }

        private void rowTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTrowInDrag)
            {
                if (newRow == null)
                {
                    newRow = RowBuilder.buildTopRow();
                    newRow.HorizontalAlignment = HorizontalAlignment.Center;
                    toolGrid.Children.Add(newRow);
                    newRow.MouseMove += rowTop_MouseMove;
                    newRow.MouseLeftButtonDown += rowTop_MouseLeftButtonDown;
                    newRow.MouseLeftButtonUp += rowTop_MouseLeftButtonUp;
                    Grid.SetRow(newRow, 2);
                }
                HandleRowDragOver(sender, e, trow);
            }
        }

        private void rowBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (isBrowInDrag)
            {
                if (newRow == null)
                {
                    newRow = RowBuilder.buildBottomRow();
                    newRow.HorizontalAlignment = HorizontalAlignment.Center;
                    toolGrid.Children.Add(newRow);
                    newRow.MouseMove += rowBottom_MouseMove;
                    newRow.MouseLeftButtonDown += rowBottom_MouseLeftButtonDown;
                    newRow.MouseLeftButtonUp += rowBottom_MouseLeftButtonUp;
                    Grid.SetRow(newRow, 4);
                }
                HandleRowDragOver(sender, e, brow);
            }
        }

        private void row_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isRowInDrag)
            {
                var element = sender as Border;
                element.ReleaseMouseCapture();
                isRowInDrag = false;
                e.Handled = true;
                RemoveEmptyRows();
                RowDropLogic(RowBuilder.buildSingleRow, RowEnum.ALL, element);
            }
        }

        private void rowTop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isTrowInDrag)
            {
                var element = sender as Border;
                element.ReleaseMouseCapture();
                isTrowInDrag = false;
                e.Handled = true;
                RemoveEmptyRows();
                RowDropLogic(RowBuilder.buildTopRow, RowEnum.TOP, element);
            }
        }

        private void rowBottom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isBrowInDrag)
            {
                var element = sender as Border;
                element.ReleaseMouseCapture();
                isBrowInDrag = false;
                e.Handled = true;
                RemoveEmptyRows();
                RowDropLogic(RowBuilder.buildBottomRow, RowEnum.BOTTOM, element);
            }
        }

        private void RowDropLogic(Func<Border> buildFunc, RowEnum rowType, Border element)
        {
            if (!inDelete)
            {
                {
                    Row newRow = new Row();
                    if (borderParent.ContainsKey(element))
                    {
                        newRow = borderParent[element];
                        newRow.RowBorder.MouseDown -= SelectRow;
                        rows.Remove(newRow);
                        toolGrid.Children.Remove(element);
                    }

                    if (emptyRowSnapped != null)
                    {
                        newRow.RowBorder = buildFunc();
                        newRow.RowBorder.MouseDown += SelectRow;
                        newRow.RowBorder.Margin = new Thickness(0, 5, 0, 5);
                        newRow.RowUI = (StackPanel)newRow.RowBorder.Child;
                        newRow.RowType = GetTypeFromBorder(element);
                        if (borderParent.ContainsKey(element))
                        {
                            Rectangle emptySeatF = rowEmptySeat[borderParent[element]];
                            rowEmptySeat.Remove(newRow);
                            emptySeats.Remove(emptySeatF);
                        }
                        newRow.LeftRow = putInLeft;
                        Rectangle emptySeat = SeatBuilder.buildEmptySeat();
                        emptySeat.Visibility = Visibility.Collapsed;
                        rowEmptySeat.Add(newRow, emptySeat);
                        emptySeats.Add(emptySeat);
                        SetRowEvents(newRow, rowType);
                        borderParent.Add(newRow.RowBorder, newRow);
                        seatParent.Add(emptySeat, newRow);
                        newRow.RowUI.Children.Add(emptySeat);
                        transformsRow[newRow.RowBorder] = new TranslateTransform();
                        SetNumberLabels();
                        leftRowStack.Children.Remove(emptyRowSnapped);
                        rightRowStack.Children.Remove(emptyRowSnapped);
                        if (putInLeft)
                        {
                            leftRowStack.Children.Insert(insertLeftPosition, newRow.RowBorder);
                            rows.Insert(rowIndexToIndex(true, insertLeftPosition), newRow);
                        }
                        else
                        {
                            rightRowStack.Children.Insert(insertRightPosition, newRow.RowBorder);
                            rows.Insert(rowIndexToIndex(false, insertRightPosition), newRow);
                        }
                        HistoryAction();
                    }
                    else
                    {
                        if (borderParent.ContainsKey(element))
                        {
                            newRow.LeftRow = borderParent[element].LeftRow;

                            if (newRow.LeftRow)
                            {
                                leftRowStack.Children.Insert(insertLeftPosition, newRow.RowBorder);
                                rows.Insert(rowIndexToIndex(true, insertLeftPosition), newRow);
                            }
                            else
                            {
                                rightRowStack.Children.Insert(insertRightPosition, newRow.RowBorder);
                                rows.Insert(rowIndexToIndex(false, insertRightPosition), newRow);
                            }
                        }
                    }
                    SelectRow(newRow.RowBorder, null);
                }
                toolGrid.Children.Remove(newRow);
                transformsRow[element].X = 0;
                transformsRow[element].Y = 0;
                newRow = null;
                SelectedIndex = SelectIndexFromSelectElement();
                AdjustRowWidth();
            }
            else
            {
                if (borderParent.ContainsKey(element))
                {
                    Rectangle emptySeat = rowEmptySeat[borderParent[element]];
                    emptySeats.Remove(emptySeat);
                    rows.Remove(borderParent[element]);
                    HistoryAction();
                }
                if (SelectedElement == element)
                    SelectedElement = null;

                toolGrid.Children.Remove(element);
                trash.Opacity = 1;
            }
            SetNumberLabels();
            RemoveEmptyRows();
        }

        private RowEnum GetTypeFromBorder(Border border)
        {
            if (border.BorderThickness.Top > border.BorderThickness.Bottom) return RowEnum.TOP;
            if (border.BorderThickness.Top < border.BorderThickness.Bottom) return RowEnum.BOTTOM;
            return RowEnum.ALL;
        }

        private void SetRowEvents(Row newRow, RowEnum rowType)
        {
            if(rowType == RowEnum.ALL)
            {
                newRow.RowBorder.MouseLeftButtonDown += row_MouseLeftButtonDown;
                newRow.RowBorder.MouseMove += row_MouseMove;
                newRow.RowBorder.MouseLeftButtonUp += row_MouseLeftButtonUp;
            }
            else if (rowType == RowEnum.TOP)
            {
                newRow.RowBorder.MouseLeftButtonDown += rowTop_MouseLeftButtonDown;
                newRow.RowBorder.MouseMove += rowTop_MouseMove;
                newRow.RowBorder.MouseLeftButtonUp += rowTop_MouseLeftButtonUp;
            }
            else
            {
                newRow.RowBorder.MouseLeftButtonDown += rowBottom_MouseLeftButtonDown;
                newRow.RowBorder.MouseMove += rowBottom_MouseMove;
                newRow.RowBorder.MouseLeftButtonUp += rowBottom_MouseLeftButtonUp;
            }
        }

        bool putInLeft = false;
        private void HandleRowDragOver(object sender, MouseEventArgs e, Border rowDragged)
        {
            var element = sender as Border;
            currentPoint = e.GetPosition(null);
            bool was = false;
            int i = 0;
            bool wasDeleted = false;
            foreach (Border emptyRow in emptyRows)
            {
                Point tl = GetPointOfControl(emptyRow);
                emptyRow.Visibility = Visibility.Visible;
                Rect rect = new Rect(new Point(tl.X - 5, tl.Y - 5), new Point(tl.X + RowBuilder.W, tl.Y + RowBuilder.H));

                if (rect.Contains(currentPoint) && emptyRow.Width > 0)
                {
                    Point p = GetPointOfControl(element);
                    if (!snapped)
                    {
                        transformsRow[element].X += tl.X - p.X;
                        transformsRow[element].Y += tl.Y - p.Y;
                        element.RenderTransform = transformsRow[element];
                        anchorPoint = new Point(tl.X + RowBuilder.W / 2, tl.Y + RowBuilder.H / 2);
                        snapped = true;
                        emptyRowSnapped = emptyRow;
                        if (i == 0) putInLeft = true;
                        else putInLeft = false;
                    }
                    was = true;
                }
                i++;
            }
            {
                Point tl = GetPointOfControl(trash);
                Rect rect = new Rect(new Point(tl.X - 50, tl.Y - 50), new Point(tl.X + 80, tl.Y + 80));
                if (rect.Contains(currentPoint) && element != brow && element != row && element != trow)
                {

                    Point p = GetPointOfControl(element);
                    if (!inDelete)
                    {
                        trash.Opacity = 0.5;
                        inDelete = true;
                    }
                    wasDeleted = true;
                }
            }
            if (!snapped)
            {
                if (!transformsRow.ContainsKey(element)) return;
                transformsRow[element].X += currentPoint.X - anchorPoint.X;
                transformsRow[element].Y += currentPoint.Y - anchorPoint.Y;
                element.RenderTransform = transformsRow[element];
                anchorPoint = currentPoint;
            }
            else
            {
                if (!was)
                {
                    if (!transformsRow.ContainsKey(element)) return;
                    snapped = false;
                    emptyRowSnapped = null;
                    transformsRow[element].X += currentPoint.X - anchorPoint.X;
                    transformsRow[element].Y += currentPoint.Y - anchorPoint.Y;
                    element.RenderTransform = transformsRow[element];
                    anchorPoint = currentPoint;
                }
            }
            if (inDelete && !wasDeleted)
            {
                inDelete = false;
                trash.Opacity = 1;
            }
        }

        List<Border> emptyRows = new List<Border>();
        int insertLeftPosition = 0;
        int insertRightPosition = 0;
        private void AddEmptyRows(Func<Border> buildFunc)
        {
            var emptyRow1 = buildFunc();
            emptyRow1.Opacity = 0.5;
            if (leftRowStack.Children.Count >= 7)
            {
                emptyRow1.Width = 0;
                emptyRow1.Height= 0;
            }
            emptyRows.Add(emptyRow1);

            var emptyRow2 = buildFunc();
            emptyRow2.Opacity = 0.5;
            if (rightRowStack.Children.Count >= 7)
            {
                emptyRow2.Width = 0;
                emptyRow2.Height = 0;
            }
            emptyRows.Add(emptyRow2);


            leftRowStack.Children.Insert(0, emptyRow1);
            rightRowStack.Children.Insert(0, emptyRow2);

            insertLeftPosition = 0;
            insertRightPosition = 0;
        }

        
        private void RemoveEmptyRows()
        {
            foreach (Border row in emptyRows)
            {
                leftRowStack.Children.Remove(row);
                rightRowStack.Children.Remove(row);
            }
            emptyRows.Clear();
        }

        public void Help_Click(object sender, EventArgs e)
        {
            var wnd = (MainWindow)Window.GetWindow(this);
            wnd.CommandBinding_Executed(sender, null);
        }

        public void SetHelpKey(object sender, EventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                HelpProvider.SetHelpKey((DependencyObject)focusedControl, "seats");
            }
        }

        // MODEL

        class Row
        {
            public StackPanel RowUI { get; set; }
            public Border RowBorder { get; set; }
            public List<Rectangle> Seats { get; set; }
            public RowEnum RowType { get; set; }
            public bool LeftRow { get; set; }

            public Row()
            {
                Seats = new List<Rectangle>();
            }
            public Row DeepCopy()
            {
                Row row = new Row();
                row.RowUI = new StackPanel();
                row.RowUI.Width = RowUI.Width;
                row.RowUI.Height = RowUI.Height;
                row.RowUI.Orientation = RowUI.Orientation;
                row.RowUI.Background = RowUI.Background;

                row.RowBorder = new Border();
                row.RowBorder.Width = RowBorder.Width;
                row.RowBorder.Height = RowBorder.Height;
                row.RowBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                row.RowBorder.BorderThickness = RowBorder.BorderThickness;

                row.RowBorder.HorizontalAlignment = RowBorder.HorizontalAlignment;

                row.RowBorder.Child = row.RowUI;

                row.RowType = RowType;
                row.LeftRow = LeftRow;

                row.Seats = new List<Rectangle>();
                foreach (var seat in Seats)
                {
                    var newSeat = new Rectangle();
                    newSeat.Width = seat.Width;
                    newSeat.Height = seat.Height;
                    newSeat.Stroke = seat.Stroke;
                    newSeat.Fill = seat.Fill;
                    newSeat.Margin = seat.Margin;
                    row.RowUI.Children.Add(newSeat);
                    row.Seats.Add(seat);
                }

                return row;
            }
        }


        class SeatBuilder
        {
            public static int W = 50;
            public static int H = 50;
            public static int M = 5;

            public static ImageBrush ImageBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/seat.png"))
            };

            public static ImageBrush LowOpacityImageBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/seat.png")),
                Opacity = 0.3
            };

            
            public static Rectangle buildSeat()
            {
                Rectangle rect = new Rectangle();
                
                rect.Fill = new SolidColorBrush(Colors.Chocolate);
                rect.Fill = ImageBrush;
                rect.Width = W;
                rect.Height = H;
                rect.Cursor = Cursors.Hand;
                rect.Margin = new Thickness(5, 5, 5, 5);
                return rect;
            }

            public static Rectangle buildEmptySeat()
            {
                Rectangle rect = new Rectangle();
                rect.Fill = new SolidColorBrush(Colors.Chocolate);
                rect.Fill = LowOpacityImageBrush;
                rect.Width = W;
                rect.Height = H;
                rect.Cursor = Cursors.Hand;
                rect.Margin = new Thickness(5, 5, 5, 5);
                return rect;
            }

            public static Rectangle buildTrash()
            {
                ImageBrush TrashImageBrush = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/trash.png"))
                };
                Rectangle rect = new Rectangle();
                // rect.Fill = new SolidColorBrush(Colors.Chocolate);
                rect.Fill = TrashImageBrush;
                rect.Width = W;
                rect.Height = H;
                rect.Cursor = Cursors.Hand;
                return rect;
            }
        }

        class RowBuilder
        {
            public static int W = 150;
            public static int H = 60;
            public static SolidColorBrush RowBrush = new((Color)ColorConverter.ConvertFromString("#d5d5ea"));

            public static Border buildSingleRow()
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Width = W;
                stackPanel.Height = H;
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                Border border = createAllBorder();
                border.Child = stackPanel;
                stackPanel.Background = RowBrush;
                return border;
            }

            public static Border buildTopRow()
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Width = W;
                stackPanel.Height = H;
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                Border border = createTopBorder();
                border.Child = stackPanel;
                stackPanel.Background = RowBrush;
                return border;
            }

            public static Border buildBottomRow()
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Width = W;
                stackPanel.Height = H;
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                Border border = createBottomBorder();
                border.Child = stackPanel;
                stackPanel.Background = RowBrush;
                return border;
            }

            public static Border createAllBorder()
            {
                Border border = new Border();
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                border.BorderThickness = new Thickness(5, 5, 5, 5);
                border.Width = W;
                border.Height = H;
                border.HorizontalAlignment = HorizontalAlignment.Left;
                return border;
            }

            public static Border createTopBorder()
            {
                Border border = new Border();
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                border.BorderThickness = new Thickness(5, 5, 5, 0);
                border.Width = W;
                border.Height = H;
                border.HorizontalAlignment = HorizontalAlignment.Left;
                return border;
            }

            public static Border createBottomBorder()
            {
                Border border = new Border();
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                border.BorderThickness = new Thickness(5, 0, 5, 5);
                border.Width = W;
                border.Height = H;
                border.HorizontalAlignment = HorizontalAlignment.Left;
                return border;
            }

            internal static Border buildByType(model.RowEnum rowType)
            {
                if (rowType == model.RowEnum.ALL)
                    return buildSingleRow();
                else if (rowType == model.RowEnum.TOP)
                    return buildTopRow();
                else
                    return buildBottomRow();
            }
        }

    }
}
