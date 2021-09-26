import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { MinerService } from '../miner/miner.service';
import { Miner } from '../models/Miner';
import { Version } from '../models/Version';
import { VersionService } from '../version.service';

@Component({
  selector: 'mm-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
})
export class SidebarComponent implements OnInit {
  public miners: Observable<Miner[]>;
  public version: Version;

  constructor(
    private minerService: MinerService,
    private versionService: VersionService
  ) {}

  public async ngOnInit() {
    this.miners = this.minerService.miners;
    this.version = await this.versionService.GetVersion();
  }
}
