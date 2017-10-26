#ifndef INCLUDED_MAP_MGR
#define INCLUDED_MAP_MGR

#include "Singleton.h"
#include "BattleMap.h"

class MapMgr : public Singleton<MapMgr>
{
public:
    MapMgr();
    ~MapMgr();

    inline BattleMap * GetCurrBatMap() { return m_currBatMap; }
	BattleMap * LoadBatMap(const std::string & path_name);
	void UnloadBatMap(BattleMap *map);

	void SetMapPath(const std::string & map_path)
	{
		m_currMapPath = map_path;
	}

	std::string GetMapPath()
	{
		return m_currMapPath;
	}

private:
    
private:
    BattleMap * m_currBatMap;
	//************************************
	// set map path that will be used in LoadBatMap
	//************************************
	std::string m_currMapPath;
};
#endif //INCLUDED_MAP_MGR