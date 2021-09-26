import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Miner } from '../../models/Miner';
import { MinerService } from '../miner.service';

@Component({
  templateUrl: './monitor.component.html',
  styleUrls: ['./monitor.component.scss'],
})
export class MonitorComponent implements OnInit {
  public miners: Miner[];
  public live: boolean;
  public timeTravelTo: Date;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private minerService: MinerService
  ) {}

  public ngOnInit() {
    this.route.paramMap.subscribe(async (paramMap) => {
      this.route.queryParamMap.subscribe(async (queryParamMap) => {
        const timeTravel = queryParamMap.get('timeTravel');
        if (timeTravel) {
          this.timeTravelTo = new Date(timeTravel);
          this.live = false;
        } else {
          this.live = true;
          this.timeTravelTo = new Date();
        }
      });
      const id = paramMap.get('id');
      if (id) {
        this.miners = [await this.minerService.Get(id)];
      } else {
        this.minerService.miners.subscribe((miners) => {
          this.miners = miners;
        });
      }
    });
  }

  public TimeTravel(to: string) {
    this.router.navigate([], {
      queryParams: { timeTravel: new Date(to).toISOString() },
    });
  }

  public GoLive() {
    this.router.navigate([], { queryParams: {} });
  }
}
