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

namespace HCIProjekat.views.customer.dialogs
{
    /// <summary>
    /// Interaction logic for TrainAddition.xaml
    /// </summary>
    public partial class SeatChooser : Page
    {
        Train Train;
        Departure Departure;
        DateOnly DepartureDate;

        List<Rectangle> seats = new List<Rectangle>();

        List<Row> rows = new List<Row>();

        Dictionary<Rectangle, Row> seatParent = new Dictionary<Rectangle, Row>();

        Dictionary<Border, Row> borderParent = new Dictionary<Border, Row>();

        Rectangle seat, newSeat, trash;

        Border newRow;

        object SelectedElement;

        SolidColorBrush highlightBrush = new SolidColorBrush(Colors.Gray);
        SolidColorBrush regularBrush = new SolidColorBrush(Colors.Black);


        List<model.Seat> chosenSeats = new List<Seat>();


        int SelectedIndex = -1;

        public SeatChooser()
        {
            InitializeComponent();
            this.Focus();
            SetNumberLabels();
            AdjustRowWidth();
        }

        public SeatChooser(Train train, Departure departure, DateOnly departureDate)
        {
            this.Train = train;
            this.Departure = departure;
            this.DepartureDate = departureDate;
            InitializeComponent();
            this.Focus();
            ConvertTrainToUI(train);
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (chosenSeats.Count == 0)
            {
                System.Windows.MessageBox.Show(
                    "Niste izabrali nijedno sedište.",
                    "Greška", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(
                    "Da li ste sigurni da želite kupiti karte za izabrana sedišta?",
                    "Potvrda kupovine", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    BuyTickets();
                    System.Windows.MessageBox.Show(
                        "Uspešno ste kupili karte!",
                        "Potvrda uspešne kupovine", System.Windows.MessageBoxButton.OK);
                }
            }
        }

        private void BuyTickets()
        {
            List<Ticket> tickets = new List<Ticket>();
            chosenSeats.ForEach(seat =>
            {
                tickets.Add(new Ticket(Train, Departure, Database.CurrentUser, seat, DepartureDate));
            });
            Database.AddTickets(tickets);
        }

        void ConvertTrainToUI(Train train)
        {
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
                    // logika da li je slobodno
                    seat.MouseLeftButtonDown += ChooseSeat;
                    seatParent.Add(seat, newRow);
                    return seat;
                }
            ).ToList());
            newRow.RowBorder.Margin = new Thickness(0, 5, 0, 5);
            if (left)
                leftRowStack.Children.Add(newRow.RowBorder);
            else
                rightRowStack.Children.Add(newRow.RowBorder);
            rows.Add(newRow);
        }

        private void ChooseSeat(object sender, EventArgs e)
        {
            // logika da li je slobodno
            Rectangle seat = (Rectangle)sender;
            bool toAdd = false;
            if(seat.Opacity == 0.5)
            {
                toAdd = false;
                seat.Opacity = 1;
            }
            else
            {
                toAdd = true;
                seat.Opacity = 0.5;
            }
            int lefts = 0;
            int rights = 0;
            int maxLeft = 0;
            int maxRight = 0;
            bool inLeft = false;
            bool found = false;
            string seatLabel = "";
            int seatOrder = 0;
            foreach (var row in rows)
            {
                if (row.LeftRow)
                {
                    lefts++;
                    maxLeft = Math.Max(maxLeft, row.Seats.Count);
                }
                else
                {
                    rights++;
                    maxRight = Math.Max(maxRight, row.Seats.Count);
                }
                
                found = false;
                seatOrder = 0;
                foreach (var rowSeat in row.Seats)
                {
                    if (rowSeat == seat)
                    {
                        inLeft = row.LeftRow;
                        found = true;
                        break;
                    }
                    seatOrder++;
                }
                if (found) break;
            }
            if (!found) return;

            if (inLeft) seatLabel = $"{(char)('A' + seatOrder)}-{lefts}-L";
            else seatLabel = $"{(char)('A' + maxLeft + seatOrder)}-{rights}-R";

            System.Diagnostics.Debug.WriteLine($"CLICKED SEAT {seatLabel}");
            // add to list or remove from list
            chosenSeats.ForEach(x => System.Diagnostics.Debug.WriteLine($" {x.Label} "));
            // aloooooooooo
            Train.PrintSeatLabels();

            if(toAdd)
            {
                System.Diagnostics.Debug.WriteLine($"DATABASE SEAT {Database.GetSeatFromTrain(Train, seatLabel)}");
                chosenSeats.Add(Database.GetSeatFromTrain(Train, seatLabel));
            }
            else
            {
                int index = chosenSeats.FindIndex(x => x.Label == seatLabel);
                chosenSeats.RemoveAt(index);
            }
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
            borderParent.Clear();
            rows.Clear();
            seats.Clear();
            SelectedElement = null;
            SelectedIndex = -1;
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
