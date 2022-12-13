using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace DS2_DEATH_COUNT.memory
{

    internal class mem
    {
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr nSize, out ulong lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] IntPtr lpBuffer, UIntPtr nSize, out ulong lpNumberOfBytesRead);

        [DllImport("User32.dll", EntryPoint = "GetAsyncKeyState")]
        private static extern short GetAsyncKeyState(int vk);



        public static bool GetKey(int vk)
        {
            //byte[] result = BitConverter.GetBytes(GetAsyncKeyState(vk));
            // return (result[0] == 1) && ( result[1] == 0x80); // Key has down after another call of GetState
            return (GetAsyncKeyState(vk) & 0x01) != 0;  
            
        }


        static bool ReadPointer(IntPtr hProc, UIntPtr baseAddress, List<uint> offsets, [Out] byte[] buffers, UIntPtr sizebuffer)
        {
            byte[] memory = new byte[8];
            if(!ReadProcessMemory(hProc, baseAddress, memory, (UIntPtr)8, IntPtr.Zero))
                return false;
            long newAddressBase = BitConverter.ToInt64(memory);

            for (var x = 0; x < offsets.Count - 1; x++) 
            {
                newAddressBase = BitConverter.ToInt64(memory) + offsets[x];
                if (!ReadProcessMemory(hProc, (UIntPtr)newAddressBase, memory, (UIntPtr)8, IntPtr.Zero))
                    return false;

            }
            newAddressBase = BitConverter.ToInt64(memory) + offsets[offsets.Count - 1];
            if (!ReadProcessMemory(hProc, (UIntPtr)newAddressBase, buffers, sizebuffer, IntPtr.Zero))
                return false;
            return true;
        }


        public static bool ReadPointerInteger(IntPtr hProc, UIntPtr baseAddress, List<uint> offsets, out int Buffer)
        {
            byte[] MemoryFinal = new byte[4];

            bool rets = ReadPointer(hProc, baseAddress, offsets, MemoryFinal, (UIntPtr)4);
            Buffer = BitConverter.ToInt32(MemoryFinal);
            return rets;
        }

        public static Process? GetProcessByName(string name)
        {

            var pc = Process.GetProcessesByName(name);
            if (pc.Length > 0)
                return pc[0];

            return null;
        }


        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "FindWindowA")]
        public static extern IntPtr FindWindowA(string ClassName, string Title);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        public struct RECT
        {
            public int direita, topo, esquerda, baixo;
        }


        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
    }
}
