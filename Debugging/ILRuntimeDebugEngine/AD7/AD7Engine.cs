﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Debugger.Interop;
using System.Runtime.InteropServices;

namespace ILRuntimeDebugEngine.AD7
{
    [ComVisible(true)]
    [Guid("552856E6-0F4F-4D79-83A5-9AF4A1A7A4B0")]
    public class AD7Engine : IDebugEngine2, IDebugEngineLaunch2, IDebugProgram3
    {
        EngineCallback callback;
        IDebugProcess2 process;
        private Guid _programId;

        internal EngineCallback Callback { get { return callback; } }
        internal IDebugProcess2 RemoteProcess { get { return process; } }
        internal bool ProgramCreateEventSent
        {
            get;
            private set;
        }

        public int Attach(IDebugEventCallback2 pCallback)
        {
            throw new NotImplementedException();
        }

        public int CanDetach()
        {
            return Constants.S_OK;
        }

        public int CauseBreak()
        {
            throw new NotImplementedException();
        }

        public int Continue(IDebugThread2 pThread)
        {
            throw new NotImplementedException();
        }

        public int Detach()
        {
            throw new NotImplementedException();
        }

        public int EnumCodeContexts(IDebugDocumentPosition2 pDocPos, out IEnumDebugCodeContexts2 ppEnum)
        {
            throw new NotImplementedException();
        }

        public int EnumCodePaths(string pszHint, IDebugCodeContext2 pStart, IDebugStackFrame2 pFrame, int fSource, out IEnumCodePaths2 ppEnum, out IDebugCodeContext2 ppSafety)
        {
            throw new NotImplementedException();
        }

        public int EnumModules(out IEnumDebugModules2 ppEnum)
        {
            throw new NotImplementedException();
        }

        public int EnumThreads(out IEnumDebugThreads2 ppEnum)
        {
            throw new NotImplementedException();
        }

        public int Execute()
        {
            throw new NotImplementedException();
        }

        public int ExecuteOnThread(IDebugThread2 pThread)
        {
            throw new NotImplementedException();
        }

        public int GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            throw new NotImplementedException();
        }

        public int GetDisassemblyStream(enum_DISASSEMBLY_STREAM_SCOPE dwScope, IDebugCodeContext2 pCodeContext, out IDebugDisassemblyStream2 ppDisassemblyStream)
        {
            throw new NotImplementedException();
        }

        public int GetENCUpdate(out object ppUpdate)
        {
            throw new NotImplementedException();
        }

        public int GetEngineInfo(out string pbstrEngine, out Guid pguidEngine)
        {
            throw new NotImplementedException();
        }

        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            throw new NotImplementedException();
        }

        public int GetName(out string pbstrName)
        {
            pbstrName = null;
            return Constants.E_NOTIMPL;
        }

        public int GetProcess(out IDebugProcess2 ppProcess)
        {
            throw new NotImplementedException();
        }

        public int GetProgramId(out Guid pguidProgramId)
        {
            pguidProgramId = _programId;
            return Constants.S_OK;
        }

        public int Step(IDebugThread2 pThread, enum_STEPKIND sk, enum_STEPUNIT Step)
        {
            throw new NotImplementedException();
        }

        public int Terminate()
        {
            return Constants.S_OK;
        }

        public int WriteDump(enum_DUMPTYPE DUMPTYPE, string pszDumpUrl)
        {
            throw new NotImplementedException();
        }

        int IDebugEngine2.Attach(IDebugProgram2[] rgpPrograms, IDebugProgramNode2[] rgpProgramNodes, uint celtPrograms, IDebugEventCallback2 pCallback, enum_ATTACH_REASON dwReason)
        {
            rgpPrograms[0].GetProgramId(out _programId);
            Callback.EngineCreated();
            Callback.ProgramCreated();

            this.ProgramCreateEventSent = true;

            return Constants.S_OK;
        }

        int IDebugEngineLaunch2.CanTerminateProcess(IDebugProcess2 pProcess)
        {
            return Constants.S_OK;
        }

        int IDebugEngine2.CauseBreak()
        {
            throw new NotImplementedException();
        }

        int IDebugEngine2.ContinueFromSynchronousEvent(IDebugEvent2 pEvent)
        {
            return Constants.S_OK;
        }

        int IDebugEngine2.CreatePendingBreakpoint(IDebugBreakpointRequest2 pBPRequest, out IDebugPendingBreakpoint2 ppPendingBP)
        {
            AD7PendingBreakPoint breakpoint = new AD7PendingBreakPoint(this, pBPRequest);
            ppPendingBP = breakpoint;

            return Constants.S_OK;
        }

        int IDebugEngine2.DestroyProgram(IDebugProgram2 pProgram)
        {
            throw new NotImplementedException();
        }

        int IDebugEngine2.EnumPrograms(out IEnumDebugPrograms2 ppEnum)
        {
            throw new NotImplementedException();
        }

        int IDebugEngine2.GetEngineId(out Guid pguidEngine)
        {
            pguidEngine = new Guid(EngineConstants.EngineGUID);
            return Constants.S_OK;
        }

        int IDebugEngineLaunch2.LaunchSuspended(string pszServer, IDebugPort2 pPort, string pszExe, string pszArgs, string pszDir, string bstrEnv, string pszOptions, enum_LAUNCH_FLAGS dwLaunchFlags, uint hStdInput, uint hStdOutput, uint hStdError, IDebugEventCallback2 pCallback, out IDebugProcess2 ppProcess)
        {
            ppProcess = new AD7Process(pPort);
            this.process = ppProcess;
            callback = new AD7.EngineCallback(this, pCallback);
            return 0;
        }

        int IDebugEngine2.RemoveAllSetExceptions(ref Guid guidType)
        {
            throw new NotImplementedException();
        }

        int IDebugEngine2.RemoveSetException(EXCEPTION_INFO[] pException)
        {
            throw new NotImplementedException();
        }

        int IDebugEngineLaunch2.ResumeProcess(IDebugProcess2 pProcess)
        {
            IDebugPort2 port;
            pProcess.GetPort(out port);
            Guid id;
            pProcess.GetProcessId(out id);
            var defaultPort = (IDebugDefaultPort2)port;
            IDebugPortNotify2 notify;
            defaultPort.GetPortNotify(out notify);

            int result = notify.AddProgramNode(new AD7ProgramNode(id));
            return Constants.S_OK;
        }

        int IDebugEngine2.SetException(EXCEPTION_INFO[] pException)
        {
            throw new NotImplementedException();
        }

        int IDebugEngine2.SetLocale(ushort wLangID)
        {
            return 0;
        }

        int IDebugEngine2.SetMetric(string pszMetric, object varValue)
        {
            return Constants.S_OK;
        }

        int IDebugEngine2.SetRegistryRoot(string pszRegistryRoot)
        {
            return 0;
        }

        int IDebugEngineLaunch2.TerminateProcess(IDebugProcess2 pProcess)
        {
            Callback.ProgramDestroyed(this);
            return Constants.S_OK;
        }
    }
}
