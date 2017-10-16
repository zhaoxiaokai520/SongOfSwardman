#ifndef INCLUDED_SINGLETON_CPP
#define INCLUDED_SINGLETON_CPP

template<class T>
class Singleton
{
public:
    static T* Instance()
    {
        if (NULL == m_pInstance)
        {
            // 二次检查  
            if (NULL == m_pInstance)
            {
                m_pInstance = new T;
                atexit(Destory);
            }
        }

        return m_pInstance;
    }

protected:
    Singleton() {} //防止实例  
    Singleton(const Singleton&) {} //防止拷贝构造一个实例  
    Singleton& operator=(const Singleton&) {} //防止赋值出另一个实例  

    virtual ~Singleton()
    {
    }

    static void Destory()
    {
        if (m_pInstance)
        {
            delete m_pInstance;
            m_pInstance = NULL;
        }
    }

private:
    static T* m_pInstance;
};

#endif // INCLUDED_SINGLETON_CPP