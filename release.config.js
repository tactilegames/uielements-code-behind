module.exports = {
  branches: [
    'master'
  ],
  plugins: [
    ['@semantic-release/commit-analyzer', {
      preset: 'conventionalcommits',
      releaseRules: [
        { type: 'feat', release: 'minor' },
        { type: 'refactor', release: 'minor' },
        { type: 'fix', release: 'patch' },
        { type: 'style', release: 'patch' },
        { type: 'docs', release: 'patch' },
        { type: 'perf', release: 'patch' },
        { type: 'test', release: 'patch' }
      ]
    }],
    ['@semantic-release/release-notes-generator', {
      preset: 'conventionalcommits',
      presetConfig: {
        types: [
          { type: 'feat', section: 'Features' },
          { type: 'refactor', section: 'Refactoring', hidden: false },
          { type: 'fix', section: 'Bug Fixes' },
          { type: 'style', section: 'Code Style' },
          { type: 'docs', section: 'Documentation', hidden: false },
          { type: 'perf', section: 'Performance', hidden: false },
          { type: 'test', section: 'Tests', hidden: false }
        ]
      }
    }],
    ['@tactilegames/semantic-pr-changelog', {
      changelogFile: 'CHANGELOG.md',
      changelogTitle: "# com.tactilegames.uielements-code-behind",
      githubOwner: process.env.CIRCLE_PROJECT_USERNAME,
      githubRepo: process.env.CIRCLE_PROJECT_REPONAME,
      types: [
        { type: 'feat', section: 'Feature' },
        { type: 'refactor', section: 'Refactoring' },
        { type: 'fix', section: 'Bug Fix' },
        { type: 'test', section: 'Test' },
        { type: 'docs', section: 'Documentation' },
        { type: 'perf', section: 'Performance' },
        { type: 'test', section: 'Tests' }
      ]
    }],
    ['@semantic-release/npm', {
      npmPublish: true
    }],
    ['@semantic-release/git', {
      assets: [
        'CHANGELOG.md',
        'package.json',
        'package-lock.json'
      ]
    }],
    ['@semantic-release/github', {
      successComment: 'This ${issue.pull_request ? "pull request" : "issue"} is included in version ${nextRelease.version}',
      failComment: false
    }],
    [
      "semantic-release-slack-bot",
      {
        "notifyOnSuccess": true,
        "notifyOnFail": false,
        "markdownReleaseNotes": true
      }
    ],
  ]
};