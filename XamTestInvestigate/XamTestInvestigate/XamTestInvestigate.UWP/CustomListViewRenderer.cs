using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;
using XamTestInvestigate.UWP;
using XamTestInvestigate;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;
using Windows.ApplicationModel.DataTransfer;
using System.Collections.ObjectModel;
using Windows.Storage;
using System.Diagnostics;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

[assembly: ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]
namespace XamTestInvestigate.UWP
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        ListView listView;
        ObservableCollection<DataSource> targetDS;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            listView = Control as ListView;

            if (e.OldElement != null)
            {
                // Unsubscribe
                listView.SelectionChanged -= OnSelectedItemChanged;
                listView.DragItemsStarting -= ListView_DragItemsStarting;
                listView.DragItemsCompleted -= ListView_DragItemsCompleted;
                listView.Drop -= ListView_Drop;
                listView.DragOver -= ListView_DragOver;
            }

            if (e.NewElement != null)
            {
                listView.SelectionMode = ListViewSelectionMode.Single;
                listView.IsItemClickEnabled = false;
                listView.CanDragItems = true;
                listView.CanReorderItems = true;
                listView.AllowDrop = true;
                
                listView.ItemsSource = ((CustomListView)e.NewElement).Items;
                // Subscribe
                listView.SelectionChanged += OnSelectedItemChanged;
                listView.DragItemsStarting += ListView_DragItemsStarting;
                listView.DragItemsCompleted += ListView_DragItemsCompleted;
                listView.Drop += ListView_Drop;
                listView.DragOver += ListView_DragOver;
            }
        }

        private void ListView_DragOver(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "Card moved to another ListView...";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;

            if (e.OriginalSource is ListView list)
            {
              targetDS = list.ItemsSource as ObservableCollection<DataSource>;
            }
        }

        private async void ListView_Drop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var text = await e.DataView.GetStorageItemsAsync();
                var storageFile = text[0] as StorageFile;
                var txt = await Windows.Storage.FileIO.ReadTextAsync(storageFile);

                var index = GetDropPositionIndex(sender, e);
                
                targetDS.Insert(index, new DataSource() { DataGuid = new Guid(), Title = "Unknown", Message = txt, CardColor = Xamarin.Forms.Color.Blue });
            }
            else
            {
                if (e.DataView != null &&
        e.DataView.Properties != null &&
        e.DataView.Properties.Any(x => x.Key == "item" && x.Value.GetType() == typeof(DataSource)))
                {
                    try
                    {
                        var def = e.GetDeferral();

                        var item = e.Data.Properties.FirstOrDefault(x => x.Key == "item");
                        var sList = e.Data.Properties.FirstOrDefault(x => x.Key == "senderList");

                        var index = GetDropPositionIndex(sender, e);

                        var card = item.Value as DataSource;
                        var senderList = (sList.Value as ListView).ItemsSource as ObservableCollection<DataSource>;

                        targetDS.Insert(index, card);
                        senderList.Remove(card);

                        def.Complete();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }
        }

        private int GetDropPositionIndex(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            var scrollViewer = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(listView, 0), 0) as ScrollViewer;
            var position = e.GetPosition((ListView)sender);
            var positionY = scrollViewer.VerticalOffset + position.Y;

            return GetItemIndex(positionY, listView);
        }

        private int GetItemIndex(double positionY, ListView targetListView)
        {
            var index = 0;
            double height = 0;

            foreach (var item in targetListView.Items)
            {
                height += GetRowHeight(item, targetListView);
                if (height > positionY) return index;
                index++;
            }

            return index;
        }

        double GetRowHeight(object listItem, ListView targetListView)
        {
            var listItemContainer = targetListView.ContainerFromItem(listItem) as ListViewItem;
            var height = listItemContainer.ActualHeight;
            var marginTop = listItemContainer.Margin.Top;

            return marginTop + height;
        }

        private void ListView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
           var selectedItem = listView.SelectedItem as DataSource;
           var itemList = listView.ItemsSource as ObservableCollection<DataSource>;

           itemList.Remove(selectedItem);
        }

        private void ListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            if (e.Items != null && e.Items.Any())
            {
                e.Data.Properties.Add("item", e.Items.FirstOrDefault());
                e.Data.Properties.Add("senderList", sender);
            }
        }

        void OnSelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            ((CustomListView)Element).NotifyItemSelected(listView.SelectedItem);
        }
    }
}
