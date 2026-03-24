
using System.Collections.ObjectModel;
using EEPROMParser.Model;
using Microsoft.VisualBasic;

namespace EEPROMParser.Controller;


public class RegionGroupsViewModel : ObservableCollection<RegionGroup>
{
    private RegionGroupsViewModel(List<RegionGroup> list) : base(list)
    {
        
    }

    public static async Task<RegionGroupsViewModel> CreateRegionGroupsViewModel()
    {
        List<RegionGroup> list = await XMLParser.CreateListRegionGroups();
        var model = new RegionGroupsViewModel(list);
        return model;
    }
    
}