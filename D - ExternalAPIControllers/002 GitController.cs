using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibGit2Sharp.Handlers;
using LibGit2Sharp;
using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Drawing;

namespace Aki32_Utilities.ExternalAPIControllers;
public class GitController
{
    // ★★★★★★★★★★★★★★★ props

    public string RemotePath { get; set; }
    public string LocalRepoDirPath { get; set; }
    private Signature TempSignature { get; init; }
    public UsernamePasswordCredentials? Credentials { private get; set; }
    public Repository Repo { get; set; }

    // ★★★★★★★★★★★★★★★ inits

    public GitController(string remotePath, string localPath, string signatureName, string signatureEmail)
    {
        RemotePath = remotePath;
        LocalRepoDirPath = localPath;
        if (string.IsNullOrEmpty(signatureName)||string.IsNullOrEmpty(signatureEmail))
            throw new UnauthorizedAccessException("signatureName and signatureEmail can not be empty");
        TempSignature = new Signature(signatureName, signatureEmail, DateTime.Now);
        Repo = GetUpToDateRepo();
    }
    ~GitController()
    {
        Repo?.Dispose();
    }


    // ★★★★★★★★★★★★★★★ methods

    public void Sync()
    {
        try
        {
            StageAll();
            CommitAll();
        }
        catch (ArgumentException)
        {
            // ok. ignore
        }
        catch (EmptyCommitException)
        {
            // ok. ignore
        }

        Pull();
        PushAll();
    }

    public void CommitAndPushAll()
    {
        try
        {
            StageAll();
            CommitAll();
        }
        catch (ArgumentException)
        {
            // ok. ignore
        }
        catch (EmptyCommitException)
        {
            // ok. ignore
        }

        PushAll();
    }

    public void WriteLogs()
    {
        foreach (var item in Repo.Commits)
        {
            Console.WriteLine();
            Console.WriteLine(item.Id);
            Console.WriteLine(item.Message);
        }
    }


    // ★★★★★★★★★★★★★★★ private methods

    private Repository GetUpToDateRepo()
    {
        if (!Directory.Exists(LocalRepoDirPath))
            Directory.CreateDirectory(LocalRepoDirPath);

        if (Directory.GetFiles(LocalRepoDirPath).Length == 0)
        {
            Clone();
            Repo = new Repository(LocalRepoDirPath);
        }
        else
        {
            Repo = new Repository(LocalRepoDirPath);
            Pull();
        }

        return Repo;
    }

    private void Clone(string branchName = "main")
    {
        var options = new CloneOptions
        {
            BranchName = branchName,
            RepositoryOperationStarting = (c) => { return true; },
            OnTransferProgress = (t) =>
            {
                Console.WriteLine($"Cloning... {t.ReceivedObjects}/{t.TotalObjects}");
                return true;
            },
        };

        Repository.Clone(RemotePath, LocalRepoDirPath, options);
    }

    private void StageAll()
    {
        var status = Repo.RetrieveStatus();
        var filePaths = status.Select(mods => mods.FilePath);
        Commands.Stage(Repo, filePaths);
    }
    private void CommitAll()
    {
        var sig = new Signature(TempSignature.Name, TempSignature.Email, DateTime.Now);

        var options = new CommitOptions()
        {
            AllowEmptyCommit = false,
        };

        Repo.Commit("updated some", sig, sig, options);
    }
    private void PushAll()
    {
        var sig = new Signature(TempSignature.Name, TempSignature.Email, DateTime.Now);

        if (Credentials == null)
            throw new UnauthorizedAccessException("You need to fill GitController.Credentials to Push to remote repo!");

        CredentialsHandler ch = (_url, _user, _cred) => Credentials;

        var options = new PushOptions()
        {
            CredentialsProvider = ch,
            OnPushTransferProgress = (p, n, t) =>
            {
                Console.WriteLine($"Pushing... {p}/{n}, {t}bytes");
                return true;
            },
        };

        Repo.Network.Push(Repo.Branches["main"], options);
    }

    private void Pull()
    {
        var sig = new Signature(TempSignature.Name, TempSignature.Email, DateTime.Now);
        var option = new PullOptions
        {
            FetchOptions = new FetchOptions
            {
                TagFetchMode = TagFetchMode.Auto,
                OnTransferProgress = (t) =>
                {
                    Console.WriteLine($"Pulling... {t.ReceivedObjects}/{t.TotalObjects}");
                    return true;
                },
            },
            MergeOptions = new MergeOptions
            {
            },
        };

        Commands.Pull(Repo, sig, option);
    }


    // ★★★★★★★★★★★★★★★

}
