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
            // ���μ��  
            if (NULL == m_pInstance)
            {
                m_pInstance = new T;
                atexit(Destory);
            }
        }

        return m_pInstance;
    }

protected:
    Singleton() {} //��ֹʵ��  
    Singleton(const Singleton&) {} //��ֹ��������һ��ʵ��  
    Singleton& operator=(const Singleton&) {} //��ֹ��ֵ����һ��ʵ��  

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