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
    public partial class TrainAddition : Page
    {


        List<Rectangle> emptySeats = new List<Rectangle>();
        List<Rectangle> seats = new List<Rectangle>();

        List<Row> rows = new List<Row>();

        Dictionary<Rectangle, Row> seatParent = new Dictionary<Rectangle, Row>();

        Dictionary<Border, Row> borderParent = new Dictionary<Border, Row>();

        Rectangle seat, newSeat;

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

        object SelectedElement;

        SolidColorBrush highlightBrush = new SolidColorBrush(Colors.Gray);
        SolidColorBrush regularBrush = new SolidColorBrush(Colors.Black);


        Dictionary<Row, Rectangle> rowEmptySeat = new Dictionary<Row, Rectangle>();

        int SelectedIndex = -1;

        Border row, trow, brow;

        public TrainAddition()
        {
            InitializeComponent();
            this.Focus();

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
            Grid.SetRow(trow, 1);


            brow = RowBuilder.buildBottomRow();
            brow.HorizontalAlignment = HorizontalAlignment.Center;
            brow.MouseLeftButtonDown += rowBottom_MouseLeftButtonDown;
            brow.MouseMove += rowBottom_MouseMove;
            brow.MouseLeftButtonUp += rowBottom_MouseLeftButtonUp;
            toolGrid.Children.Add(brow);
            Grid.SetRow(brow, 2);

            seat = SeatBuilder.buildSeat();
            toolGrid.Children.Add(seat);
            Grid.SetRow(seat, 3);
            Grid.SetZIndex(seat, 100);
            seat.MouseLeftButtonDown += root_MouseLeftButtonDown;
            seat.MouseMove += root_MouseMove;
            seat.MouseLeftButtonUp += root_MouseLeftButtonUp;


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
            foreach (var tempRow in rows)
            {
                if (tempRow.LeftRow)
                    leftRowStack.Children.Remove(tempRow.RowBorder);
                else
                    rightRowStack.Children.Remove(tempRow.RowBorder);
            }
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

        private void AdjustRowWidth(bool empty = false)
        {
            if (!empty)
            {
                foreach (var row in rows)
                {
                    row.RowUI.Width = Math.Max(row.Seats.Count * 60, 20);
                    row.RowBorder.Width = Math.Max(row.Seats.Count * 60 + 5, 20);
                }
            }
            else
            {
                foreach (var row in rows)
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
                Row clone = parent.DeepCopy();
                Rectangle emptySeat = SeatBuilder.buildEmptySeat();
                emptySeat.Visibility = Visibility.Collapsed;
                emptySeat.Margin = new Thickness(5, 5, 5, 5);
                emptySeats.Add(emptySeat);
                clone.RowUI.Children.Add(emptySeat);
                rowEmptySeat[clone] = emptySeat;
                seatParent.Add(emptySeat, clone);
                int ind = rows.IndexOf(parent);
                rows.Insert(ind, clone);
                SelectedIndex = ind;
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
                var seat = SeatBuilder.buildSeat();
                seat.Margin = new Thickness(0, 0, 5, 0);
                var emptySeat = rowEmptySeat[parent];
                parent.RowUI.Children.Remove(emptySeat);
                parent.Seats.Add(seat);
                parent.RowUI.Children.Add(seat);
                parent.RowUI.Children.Add(emptySeat);
                SetNumberLabels();
                AdjustRowWidth();
                ClearRows();
                RedrawRows();
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

        //private void row_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        //DragDrop.DoDragDrop((Border)sender, (Border)sender, DragDropEffects.Move);
        //    }
        //}

        private void SelectRow(object sender, MouseEventArgs e)
        {
            if (SelectedElement != null)
            {
                ((Border)SelectedElement).BorderBrush = regularBrush;
            }
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
        private void root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            anchorPoint = e.GetPosition(null);
            element.CaptureMouse();
            isInDrag = true;
            e.Handled = true;
        }

        private TranslateTransform transform = new TranslateTransform();
        private void root_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInDrag)
            {

                if (newSeat == null)
                {
                    newSeat = SeatBuilder.buildSeat();
                    toolGrid.Children.Add(newSeat);
                    Grid.SetRow(newSeat, 3);
                }
                HandleSeatDragOver(sender, e);
            }
        }

        private void root_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                element.ReleaseMouseCapture();
                isInDrag = false;
                e.Handled = true;
                if (emptySnapped != null)
                {
                    Rectangle newSeat = CopyRectangle(seat);
                    seatParent[emptySnapped].RowUI.Children.Remove(emptySnapped);
                    seatParent[emptySnapped].Seats.Add(newSeat);
                    seatParent[emptySnapped].RowUI.Children.Add(newSeat);
                    seatParent[emptySnapped].RowUI.Children.Add(emptySnapped);
                    newSeat.Margin = new Thickness(5, 5, 5, 5);
                    SetNumberLabels();
                }
                toolGrid.Children.Remove(newSeat);
                transform.X = 0;
                transform.Y = 0;
                newSeat = null;
                foreach (Rectangle emptySeat in emptySeats)
                {
                    emptySeat.Visibility = Visibility.Collapsed;
                }
                AdjustRowWidth();
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
        Rectangle emptySnapped;
        private void HandleSeatDragOver(object sender, MouseEventArgs e)
        {
            var element = sender as FrameworkElement;
            currentPoint = e.GetPosition(null);
            bool was = false;
            foreach (Rectangle emptySeat in emptySeats)
            {
                Row parent = seatParent[emptySeat];
                Point tl = GetPointOfControl(emptySeat);
                emptySeat.Visibility = Visibility.Visible;
                Rect rect = new Rect(new Point(tl.X - 5, tl.Y - 5), new Point(tl.X + 55, tl.Y + 55));
                if (rect.Contains(currentPoint))
                {
                    Point p = GetPointOfControl(seat);
                    if (!snapped)
                    {
                        transform.X += tl.X - p.X;
                        transform.Y += tl.Y - p.Y;
                        seat.RenderTransform = transform;
                        anchorPoint = new Point(tl.X + SeatBuilder.W / 2, tl.Y + SeatBuilder.H  / 2);
                        snapped = true;
                        emptySnapped = emptySeat;
                    }
                    was = true;
                }
            }
            if (!snapped)
            {
                transform.X += currentPoint.X - anchorPoint.X;
                transform.Y += currentPoint.Y - anchorPoint.Y;
                seat.RenderTransform = transform;
                anchorPoint = currentPoint;
            }
            else
            {
                if (!was)
                {
                    snapped = false;
                    emptySnapped = null;
                    transform.X += currentPoint.X - anchorPoint.X;
                    transform.Y += currentPoint.Y - anchorPoint.Y;
                    seat.RenderTransform = transform;
                    anchorPoint = currentPoint;
                }
            }
            AdjustRowWidth(true);
        }

        private Point GetPointOfControl(UIElement rootVisual)
        {
            Point relativePoint = rootVisual.TransformToAncestor(Window.GetWindow(this)).Transform(new Point(0, 0));
            return relativePoint;
        }

        // MOVE ROW

        Border emptyRowSnapped;
        private void row_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            transformRow = new TranslateTransform();
            var element = sender as FrameworkElement;
            anchorPoint = e.GetPosition(null);
            element.CaptureMouse();
            isInDrag = true;
            e.Handled = true;
            RemoveEmptyRows();
            AddEmptyRows(RowBuilder.buildSingleRow);
        }

        private void rowTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            transformRow = new TranslateTransform();
            var element = sender as FrameworkElement;
            anchorPoint = e.GetPosition(null);
            element.CaptureMouse();
            isInDrag = true;
            e.Handled = true;
            RemoveEmptyRows();
            AddEmptyRows(RowBuilder.buildTopRow);
        }

        private void rowBottom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            transformRow = new TranslateTransform();
            var element = sender as FrameworkElement;
            anchorPoint = e.GetPosition(null);
            element.CaptureMouse();
            isInDrag = true;
            e.Handled = true;
            RemoveEmptyRows();
            AddEmptyRows(RowBuilder.buildBottomRow);
        }

        private TranslateTransform transformRow = new TranslateTransform();
        private void row_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInDrag)
            {
                if (newRow == null)
                {
                    newRow = RowBuilder.buildSingleRow();
                    newRow.HorizontalAlignment = HorizontalAlignment.Center;
                    toolGrid.Children.Add(newRow);
                    Grid.SetRow(newRow, 0);
                }
                HandleRowDragOver(sender, e, row);
            }
        }

        private void rowTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInDrag)
            {
                if (newRow == null)
                {
                    newRow = RowBuilder.buildTopRow();
                    newRow.HorizontalAlignment = HorizontalAlignment.Center;
                    toolGrid.Children.Add(newRow);
                    Grid.SetRow(newRow, 1);
                }
                HandleRowDragOver(sender, e, trow);
            }
        }

        private void rowBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInDrag)
            {
                if (newRow == null)
                {
                    newRow = RowBuilder.buildBottomRow();
                    newRow.HorizontalAlignment = HorizontalAlignment.Center;
                    toolGrid.Children.Add(newRow);
                    Grid.SetRow(newRow, 2);
                }
                HandleRowDragOver(sender, e, brow);
            }
        }

        private void row_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                element.ReleaseMouseCapture();
                isInDrag = false;
                e.Handled = true;
                RemoveEmptyRows();
                RowDropLogic(RowBuilder.buildSingleRow);

            }
        }

        private void rowTop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                element.ReleaseMouseCapture();
                isInDrag = false;
                e.Handled = true;
                RemoveEmptyRows();
                RowDropLogic(RowBuilder.buildTopRow);

            }
        }

        private void rowBottom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                element.ReleaseMouseCapture();
                isInDrag = false;
                e.Handled = true;
                RemoveEmptyRows();
                RowDropLogic(RowBuilder.buildBottomRow);

            }
        }

        private void RowDropLogic(Func<Border> buildFunc)
        {
            if (emptyRowSnapped != null)
            {
                Row newRow = new Row();
                newRow.LeftRow = putInLeft;
                newRow.RowBorder = buildFunc();
                newRow.RowBorder.MouseDown += SelectRow;
                newRow.RowBorder.Margin = new Thickness(0, 5, 0, 5);
                newRow.RowUI = (StackPanel)newRow.RowBorder.Child;
                newRow.RowType = RowEnum.ALL;
                Rectangle emptySeat = SeatBuilder.buildEmptySeat();
                emptySeat.Visibility = Visibility.Collapsed;
                rowEmptySeat.Add(newRow, emptySeat);
                emptySeats.Add(emptySeat);
                borderParent.Add(newRow.RowBorder, newRow);
                seatParent.Add(emptySeat, newRow);
                newRow.RowUI.Children.Add(emptySeat);
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
                SetNumberLabels();
            }
            toolGrid.Children.Remove(newRow);
            transformRow.X = 0;
            transformRow.Y = 0;
            newRow = null;
            SelectedIndex = SelectIndexFromSelectElement();
            AdjustRowWidth();
        }

        bool putInLeft = false;
        private void HandleRowDragOver(object sender, MouseEventArgs e, Border rowDragged)
        {
            var element = sender as FrameworkElement;
            currentPoint = e.GetPosition(null);
            bool was = false;
            int i = 0;
            foreach (Border emptyRow in emptyRows)
            {
                Point tl = GetPointOfControl(emptyRow);
                emptyRow.Visibility = Visibility.Visible;
                Rect rect = new Rect(new Point(tl.X - 5, tl.Y - 5), new Point(tl.X + RowBuilder.W, tl.Y + RowBuilder.H));

                if (rect.Contains(currentPoint))
                {
                    Point p = GetPointOfControl(rowDragged);
                    if (!snapped)
                    {
                        transformRow.X += tl.X - p.X;
                        transformRow.Y += tl.Y - p.Y;
                        rowDragged.RenderTransform = transformRow;
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
            if (!snapped)
            {
                transformRow.X += currentPoint.X - anchorPoint.X;
                transformRow.Y += currentPoint.Y - anchorPoint.Y;
                rowDragged.RenderTransform = transformRow;
                anchorPoint = currentPoint;
            }
            else
            {
                if (!was)
                {
                    snapped = false;
                    emptyRowSnapped = null;
                    transformRow.X += currentPoint.X - anchorPoint.X;
                    transformRow.Y += currentPoint.Y - anchorPoint.Y;
                    rowDragged.RenderTransform = transformRow;
                    anchorPoint = currentPoint;
                }
            }
        }

        List<Border> emptyRows = new List<Border>();
        int insertLeftPosition = 0;
        int insertRightPosition = 0;
        private void AddEmptyRows(Func<Border> buildFunc)
        {
            int insertPosition = 0;
            var leftInsert = false;
            if (SelectedElement != null)
            {
                var border = (Border)SelectedElement;
                var parent = borderParent[border];
                leftInsert = parent.LeftRow;
                insertPosition = indexToRowIndex(parent.LeftRow, SelectedIndex);
                Console.WriteLine($"INSERT POSITION {insertPosition}");
            }
            var emptyRow1 = buildFunc();
            emptyRow1.Opacity = 0.5;
            emptyRows.Add(emptyRow1);
            var emptyRow2 = buildFunc();
            emptyRow2.Opacity = 0.5;
            emptyRows.Add(emptyRow2);
            if (leftInsert)
            {
                leftRowStack.Children.Insert(insertPosition, emptyRow1);
                rightRowStack.Children.Insert(0, emptyRow2);
                insertLeftPosition = insertPosition;
                insertRightPosition = 0;
            }
            else
            {
                leftRowStack.Children.Insert(0, emptyRow1);
                rightRowStack.Children.Insert(insertPosition, emptyRow2);
                insertLeftPosition = 0;
                insertRightPosition = insertPosition;
            }
        }

        private void RemoveEmptyRows()
        {
            foreach (Border row in emptyRows)
            {
                if (leftRowStack.Children.Contains(row))
                    leftRowStack.Children.Remove(row);
                if (rightRowStack.Children.Contains(row))
                    rightRowStack.Children.Remove(row);
            }
            emptyRows.Clear();
        }

        // MODEL

        enum RowEnum
        {
            ALL,
            TOP,
            BOTTOM
        }

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
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/seat.jpg"))
            };

            public static ImageBrush LowOpacityImageBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/seat.jpg")),
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
                return rect;
            }
        }

        class RowBuilder
        {
            public static int W = 150;
            public static int H = 60;
            public static SolidColorBrush RowBrush = new SolidColorBrush(Colors.AliceBlue);

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

        }


    }
}
