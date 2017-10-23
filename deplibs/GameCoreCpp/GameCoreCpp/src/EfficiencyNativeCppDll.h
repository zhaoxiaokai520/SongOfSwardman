#ifndef GoWin_DLL_CLASS_EXPORTS  
//����ɵ���  
#define GoWin_DLL_CLASS __declspec(dllexport)  
#else  
//����ɵ���  
#define GoWin_DLL_CLASS __declspec(dllimport)  
#endif  
#define NPARTS 1000  
#define DIMS 3  

class GoWin_DLL_CLASS EfficiencyNativeCppDll
{
public:
    EfficiencyNativeCppDll(void);
    ~EfficiencyNativeCppDll(void);

    void InitPositions();
    void UpdatePositions();
    double ComputePot();
    double Pot;
private:
    double _r[DIMS][NPARTS];
};