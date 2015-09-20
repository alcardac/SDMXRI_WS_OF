__author__ = 'puiu'
import os, fnmatch, shutil, subprocess
import smtplib
from email.mime.text import MIMEText
from dropbox.client import DropboxClient
import zipfile


allowedDirs = [  "SdmxApi",
                 "SdmxDataParser",
                 "SdmxDataQueryWriter",
                 "SdmxEdiDataWriter",
                 "SdmxIo",
                 "SdmxMlConstants",
                 "SdmxObjects",
                 "SdmxQueryBuilder",
                 "SdmxSourceUtil",
                 "SdmxStructureMutableParser",
                 "SdmxStructureParser",
                 "SdmxStructureRetrieval",
                 "TabularWriters",
                 "XObjects",
                 "Org.Sdmx.Resources.SdmxMl.Schemas.V10",
                 "Org.Sdmx.Resources.SdmxMl.Schemas.V20",
                 "Org.Sdmx.Resources.SdmxMl.Schemas.V21"]

ignoreDirs = [".git", ".nuget", "_ReSharper.CommonAPI", "packages", "SdmxApiTests"]
extensionList = ["*.dll", "*.pdb"]

PROJECTS = ((r'E:\Devel\GIT\CommonAPI\sdmxsource.net_0_9_25', 'CommonAPI.sln', 'sdmxsource.net.zip'),
            (r'E:\Devel\GIT\CommonAPI\estat_sdmxsource_extension.net_25', 'EstatSdmxSourceExtension.sln', 'estat_sdmxsource_extension.net.zip'))


RESULT_PATH = r'E:\Devel\GIT\CommonAPI\sdmxsource.net_0_9_25\Bin'
CONFIGURATION = "Release"
BUILD_CMD = r'C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe'
GIT_CMD = r'C:\Program Files (x86)\Git\bin\git.exe'

BUILD_OUTPUT_PATH = os.path.join('bin', CONFIGURATION)

OUTPUT_BINARY_NAME = 'common-api-v0.3.0- {0}.zip'.format(CONFIGURATION)
OUTPUT_SOURCE__NAME = 'sdmxsource.net.zip'

app_key = 'f82c83itnx5m3b6'
app_secret = '1quczr366sfdqsx'
access_token = 'j_uQD1tqOFEAAAAAAAAAAbI0Lw4BALhZnvBIKMWr6aPxXmqntestjsLRhY7KenFQ'

def SendEmail():

    SUBJECT = "Common API Build"
    TO = "alexandru.puiu@intrasoft-intl.com; " + \
         "monica.barsan@intrasoft-intl.com; " + \
         "Ionela.Florea@intrasoft-intl.com; " + \
         "Dragos.balan@intrasoft-intl.com; "  + \
         "Tasos.Chronis@intrasoft-intl.com"
    FROM = "alexandru.puiu@intrasoft-intl.com"

    text = "All, \n\n" + "A new version of .NET based Common API 0.3.0 is now available on Dropbox: https://www.dropbox.com/home/Internal%20Releases/CommonAPI/v0.3.0"
    text = text + "\n\nThis message is automatically genarated."
    text = text + "\n\nPowered by Python"
    msg = MIMEText(text)
    msg['Subject'] = SUBJECT
    msg['From'] = FROM
    msg['To'] = TO


    server = smtplib.SMTP(r'ath-mailbe.athens.intrasoft-intl.private')
    server.login(r'ATH\puiu','Amanda12345@')
    server.send_message(msg)
    server.quit()

def GitExecute(command, projectPath):
    result = 0
    try:
        cmd = [GIT_CMD, command]
        result = subprocess.call(cmd, cwd=projectPath)
    except Exception as e:
        print(e)

    if result == 1:
        return False
    return True

def UpdateSources():
    for path, sol, e in PROJECTS:
        if not GitExecute('pull', path): return False
    return True

def BuildProject(projectPath):
    configuration = '/p:Configuration=%s' % CONFIGURATION
    result = 0
    print(projectPath)
    print(configuration)
    try:
        result = subprocess.call([BUILD_CMD, projectPath, configuration, '/t:Rebuild'])
    except Exception as e:
        print(e)

    if result == 1 :
        return False

    return True

def Upload2Dropbox():

    binaryPath =  os.path.join(RESULT_PATH, OUTPUT_BINARY_NAME)

    try:
        client = DropboxClient(access_token)
        with open(binaryPath, 'rb') as f:
            client.put_file('/Internal Releases/CommonAPI/v0.3.0/%s' % OUTPUT_BINARY_NAME, f, overwrite= True)
        for p, s, export in PROJECTS:
            srcPath =  os.path.join(RESULT_PATH, export)
            with open(srcPath, 'rb') as f:
                client.put_file('/Internal Releases/CommonAPI/v0.3.0/%s' % export, f, overwrite= True)

    except Exception as e:
        print(e)
        return False
    return True

def GitExport():
    for p, sol, export in PROJECTS:
        path = os.path.join(RESULT_PATH, export)
        cmd = ['archive', ' --format zip',' --output ' , path , ' HEAD']
        GitExecute(cmd, p)

def CopyBuildOutput(projectPath, resultPath=RESULT_PATH):
    file_list = []

    for dirname, dirnames, filenames in os.walk(projectPath):
        #remove the ingored directories
        for d in [i for dir in ignoreDirs for i in dirnames if dir in i]:
            dirnames.remove(d)

        for e in extensionList:
            for filename in fnmatch.filter(filenames, e):
                file_list.append(os.path.join(dirname, filename))

    if not os.path.exists(resultPath):
        os.mkdir(resultPath)

    for file in file_list:
         if BUILD_OUTPUT_PATH in file:
             new_fileName =  os.path.join(resultPath, os.path.basename(file))
             shutil.copy2(file, new_fileName )

def MakeArchive():
    try:
        os.chdir(RESULT_PATH)
        with zipfile.ZipFile(OUTPUT_BINARY_NAME, 'w') as zip:
            filelist = os.listdir(RESULT_PATH)
            for f in [file for file in filelist if file != OUTPUT_BINARY_NAME and file != OUTPUT_SOURCE__NAME]:
                zip.write(os.path.basename(f))
    except Exception as e:
        print(e)
        return False
    return True

def ClearOutputFolder():
    print("Clear ")
    try:
        if os.path.exists(RESULT_PATH):
            filelist = os.listdir(RESULT_PATH)
            for f in filelist:
                os.remove(os.path.join(RESULT_PATH, f))
    except Exception as e:
        print(e)

if __name__ == '__main__':
    print("[START BUILD]")

    ClearOutputFolder()

    if not UpdateSources():
        print("Warning: Error updating sources from GIT!")

    for path, solution, export in PROJECTS:
        if not BuildProject(os.path.join(path, solution)):
            print("Error building project: " + solution)
            exit(1)
        print("Copy build output for project: " + solution)
        CopyBuildOutput(path)

        MakeArchive()
    print("Successfully build the projects.")

    GitExport()

    if Upload2Dropbox():
        SendEmail()

    print("[FINISHED]")
