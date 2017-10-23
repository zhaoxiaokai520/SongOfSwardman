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
    void LoadBatMap(std::string path_name);

private:
    
private:
    BattleMap * m_currBatMap;
};
#endif //INCLUDED_MAP_MGR