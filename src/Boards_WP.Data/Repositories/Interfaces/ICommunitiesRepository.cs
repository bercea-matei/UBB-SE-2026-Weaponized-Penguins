using System;
using System.Collections.Generic;

public interface ICommunitiesRepository
{
    public void AddCommunity(Community NewCommunity);
    public void UpdateDescription(int CommunityID, String NewDescription);
    public void UpdateCommunityPicture(int CommunityID, byte[] NewPhoto);
    public void UpdateBanner(int CommunityID, String NewBanner);
    public void IncreaseMembersNumber(int CommunityID);
    public void DecreaseMembersNumber(int CommunityID);
    public Boolean CheckOwner(int CommunityID, int OwnerID);
    public List<Community> GetCommunitiesByNamesMatch(String Name);
    public List<Community> GetCommunitiesUserIsPartOf(int UserID);
    public Boolean IsPartOfCommunity(int UserID, int CommunityID);
}
